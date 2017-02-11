using System;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Input;

namespace Rp2d
{
	public class GameplayActionScene : GameScene
	{
		
		private bool _firstTimeEntering = true;
		private WorldMapDefinition _currentMap;
		private Hero _hero;
		public Hero Hero { get { return _hero; } }
		private Vector2? _heroStartPosition = null;
		private Vector2 _mapSize = new Vector2(0, 0);
		
#if DEBUG
		private FPSDisplayer _fpsDisplayer;
#endif
		
		public GameplayActionScene(WorldMapDefinition worldMap, Vector2 heroStartPosition) : this(worldMap)
		{
			_heroStartPosition = heroStartPosition;
		}
		
		public GameplayActionScene(WorldMapDefinition worldMap)
		{
			_currentMap = worldMap;
			
			this.Camera.SetViewFromViewport();
			
			this.ScheduleUpdate(1);
			this.RegisterDisposeOnExitRecursive();
		}
		
		~GameplayActionScene ()
		{
		}
	
		public void SetupScene()
		{
			// Setup the scene
			// Load the background
			BackgroundLoader _loader = new BackgroundLoader();
			var backgroundLayers = _loader.LoadLayers(_currentMap.MapFilename);
			
			// Load the NPCs
			string npcFile = _currentMap.MapFilename.Substring(0, _currentMap.MapFilename.LastIndexOf('.')) + ".npc";
			NpcManager.LoadNpcs(npcFile);
			
			// Load the spawn points
			string spawnPointFile = _currentMap.MapFilename.Substring(0, _currentMap.MapFilename.LastIndexOf('.')) + ".spawns";
			EnemyManager.LoadMapSpawnPoints(spawnPointFile);
			
			// The background loader returns a list of layers to draw,
			//  and a SpriteList for each texture in that layer
			foreach(var layer in backgroundLayers)
			{
				if(layer.MapSize != Vector2.Zero)
				{
					_mapSize = layer.MapSize;
				}
				
				// Layers come in alphabetical order, let's play 
				//  this up when drawing the maps
				if(layer.LayerName.Contains("Player Layer"))
				{
					foreach(var npc in NpcManager.Npcs)
					{
						CollisionManager.RegisterCollidableEntity(npc);
						AddChild(npc.EntitySprite);
					}
					
					// Let's create a hero
					_hero = new Hero();
					if(_heroStartPosition.HasValue)
					{
						_hero.EntitySprite.Position = _heroStartPosition.Value;
					}
					else
					{
						_hero.EntitySprite.Position = new Vector2(543, 1290);
					}
					
					AddChild(_hero.EntitySprite);
					
					CollisionManager.RegisterCollidableEntity(_hero);
				}
				foreach(SpriteList spriteList in layer.BackgroundSprites)
				{
					AddChild(spriteList);
				}
			}
			
			// Register the collision layer
			if(_loader.CollisionRectangles != null)
			{
				foreach(var collisionRect in _loader.CollisionRectangles)
				{
					CollisionManager.RegisterCollidableEntity(new EmptyCollisionEntity(collisionRect));
				}
			}
			
#if DEBUG
			// Draw the FPS displayer			
			_fpsDisplayer = new FPSDisplayer();
			_fpsDisplayer.Position = new Vector2(_hero.EntitySprite.Position.X - 450, 
			                                     _hero.EntitySprite.Position.Y - 320);
			AddChild(_fpsDisplayer);
			
			// Draw Map Exits
			foreach(var mapExitArea in _currentMap.WorldMapExitAreas)
			{
				var textureInfo = TextureManager.CreateTexture2D(0x88FFFFFF, mapExitArea.MapExitAreaWidth, mapExitArea.MapExitAreaHeight);
				
				var sprite = new SpriteUV(textureInfo.Info);
				sprite.Quad.S = textureInfo.Info.TextureSizef;
				sprite.Position = new Vector2(mapExitArea.MapExitAreaX, mapExitArea.MapExitAreaY);
				AddChild(sprite);
			}
#endif
			
			// Let's start the background music
			AudioManager.PlayBackgroundSong(_currentMap.MapBackgroundSongFilename);
			
		}
		
		private void SwitchScene(WorldMapExitArea mapExitArea)
		{
			CollisionManager.RemoveAll();
			NpcManager.RemoveAllNpcs();
			EnemyManager.RemoveAllEnemies();
			_hero.Cleanup();
			
			// Remove all the sprites
			base.RemoveAllChildren(true);
			base.Cleanup();
			TextureManager.DisposeTextures();
			
			// Force garbage collection
			GC.Collect();
			
			var loadingScene = new LoadingScene(typeof(GameplayActionScene), WorldMapManager.GetMapFromId(mapExitArea.MapToExitTo),
			            new Vector2(mapExitArea.MapToExitToStartingXInPixels, mapExitArea.MapToExitToStartingYInPixels));                        
				
			Director.Instance.ReplaceScene(loadingScene);
		}
		
		Vector2 _newCameraPosition;
		public override void Update (float dt)
		{
			if(_firstTimeEntering)
			{
				SetupScene();
				Camera2D.Center = _hero.EntitySprite.Position;
				_firstTimeEntering = false;
			}
			
			// Check to see if we in an area that would exit the map
			var mapExitArea = _currentMap.GetExitAreaFromPosition(_hero);
			if(mapExitArea != null)
			{
				SwitchScene(mapExitArea);
				return;
			}
			
			// Get user input
			GameInput.UpdateGameInput();
			
			// Manage enemies
			EnemyManager.Update(dt);
		
			// Update collisions
			CollisionManager.NotifyCollisions(dt);
			
			UpdateCameraPosition();
			
#if DEBUG
			_fpsDisplayer.Position = new Vector2(Camera2D.Center.X - (Director.Instance.GL.Context.GetViewport().Width / 2) + 10, 
			                                     Camera2D.Center.Y - (Director.Instance.GL.Context.GetViewport().Height / 2) - 50);
#endif
			
			// Exit on Left Bumper just for Pappas
			if(GameInput.GetInputValue(GameInputControls.Exit) != 0)
			{
				Environment.Exit(0);
			}
		}
		
		private void UpdateCameraPosition()
		{
			// For now position the camera around the hero... Maybe make this an external
			//  camera position module
			_newCameraPosition = _hero.EntitySprite.Position;
			
			if(_hero.EntitySprite.Position.Y - Director.Instance.GL.Context.GetViewport().Height / 2 < 0)
			{
				_newCameraPosition.Y = (Director.Instance.GL.Context.GetViewport().Height / 2);
			}
			if(_hero.EntitySprite.Position.Y + Director.Instance.GL.Context.GetViewport().Height / 2 > _mapSize.Y)
			{
				_newCameraPosition.Y = _mapSize.Y - (Director.Instance.GL.Context.GetViewport().Height / 2);
			}
			if(_hero.EntitySprite.Position.X - Director.Instance.GL.Context.GetViewport().Width / 2 < 0)
			{
				_newCameraPosition.X = (Director.Instance.GL.Context.GetViewport().Width / 2);
			}
			if(_hero.EntitySprite.Position.X + Director.Instance.GL.Context.GetViewport().Width / 2 > _mapSize.X)
			{
				_newCameraPosition.X = _mapSize.X - (Director.Instance.GL.Context.GetViewport().Width / 2);
			}
			
			Camera2D.Center = _newCameraPosition;
		}
		
	}
}

