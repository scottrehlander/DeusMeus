using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using System.Collections.Generic;
using System.IO;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public class BackgroundLoader
	{
		
		private string _tilesetPath = "/Application/Content/GameMaps/TileSheets/";
		private string _mapImagesPath = "/Application/Content/GameMaps/MapImages/";
		
		public List<Rectangle> CollisionRectangles { get; set; }
		
		public BackgroundLoader ()
		{
		}
		
		public List<BackgroundLayer> LoadLayers(string mapFile)
		{
			List<BackgroundLayer> backgroundLayers = new List<BackgroundLayer>();
			CollisionRectangles = new List<Rectangle>();
			
			// Read in a map file
			Squared.Tiled.Map map = Squared.Tiled.Map.Load(mapFile);
			
			List<BackgroundTiledTexture2D> mapTextures = new List<BackgroundTiledTexture2D>();
			
			// Load the sprites
			foreach(var tileset in map.Tilesets)
			{
				var tilesetKey = tileset.Key;
				var firstGid = tileset.Value.FirstTileID;
				var tileHeight = tileset.Value.TileHeight;
				var tileWidth = tileset.Value.TileWidth;
				
				// Assume a 0 margin and
				//   figure out what tile this is from our texture collection
				Texture2DAndTextureInfo info = TextureManager.CreateTexture2D(Path.Combine(_tilesetPath, tilesetKey) + ".png", false,
				                                                              new Vector2i(tileWidth, tileHeight), true);				
				var backgroundTiledTexture2D = new BackgroundTiledTexture2D()
				{
					Texture = info.Texture,
					TextureInfo = info.Info,
					TextureName = tilesetKey,
					TileHeight = tileHeight,
					TileWidth = tileWidth,
					StartingGid = firstGid,
					TextureSpriteList = new SpriteList(info.Info)
				};
				
				var endingGid = backgroundTiledTexture2D.EndingGid;
				
				mapTextures.Add(backgroundTiledTexture2D);
			}
			
			foreach(var tiledLayer in map.Layers)
			{
				// If there is a png, don't skip, otherwise if there is a 
				//  visible key and the value for visible is false, skip it
				if(!tiledLayer.Value.Properties.ContainsKey("png") &&
					tiledLayer.Value.Properties.ContainsKey("visible") && 
				   tiledLayer.Value.Properties["visible"].ToString().Equals("false"))
				{
					continue;
				}
				
				if(tiledLayer.Key.Contains("Collision Layer"))
				{
					LoadCollisionLayer(map, tiledLayer.Value);
					bool drawCollisionLayer = false;

#if DRAW_COLLISION_RECTS
					drawCollisionLayer = true;
#endif
					
					if(!drawCollisionLayer)
					{
						continue;
					}					
				}
				
				// Grab the png property (if it exists)
				if(tiledLayer.Value.Properties.ContainsKey("png"))
				{
					// Since a png exists, draw that instead of drawing
					//   the tiles on the layer
					var pngInstead = tiledLayer.Value.Properties["png"];	
					
					// Load the png texture
					var backgroundLayerTextureInfo = TextureManager.CreateTexture2D(Path.Combine(_mapImagesPath, pngInstead), false, true);                          
					
					SpriteUV sprite = new SpriteUV(backgroundLayerTextureInfo.Info);
					sprite.Position = new Vector2(0, 0);
					sprite.Quad.S = backgroundLayerTextureInfo.Info.TextureSizef;
					
					var backgroundLayerSpriteList = new SpriteList(backgroundLayerTextureInfo.Info);
					backgroundLayerSpriteList.AddChild(sprite);
					var backgroundLayer = new BackgroundLayer()
					{
						LayerName = tiledLayer.Key,
						BackgroundSprites = new List<SpriteList>() { backgroundLayerSpriteList },
						MapSize = new Vector2(map.Width * map.TileWidth, map.Height * map.TileHeight)
					};
					backgroundLayers.Add(backgroundLayer);
					
					continue;
				}
				
				// We don't have a png to draw so let's draw al the tiles in the list
				BackgroundLayer layer = new BackgroundLayer();
				layer.BackgroundSprites = new List<SpriteList>();
				layer.LayerName = tiledLayer.Key;
				layer.MapSize = new Vector2(map.Width * map.TileWidth, map.Height * map.TileHeight);
				
				// Key is the texture name, value is a list of sprites for that texture
				Dictionary<string, SpriteList> spritesForLayer = new Dictionary<string, SpriteList>();
				
				int positionX = 0;
				int positionY = map.Height * map.TileHeight - map.TileHeight;
				int tileCounter = 0;
				for(int i = 0; i < map.Height; i++)
				{
					positionX = 0;
					for(int j = 0; j < map.Width; j++)
					{
						var tile = tiledLayer.Value.Tiles[tileCounter++];
						
						if(tile > 0)
						{
							// Figure out the texture for this guy
							BackgroundTiledTexture2D mapTexture = null;
							foreach(BackgroundTiledTexture2D backgroundTexture in mapTextures)
							{
								if(tile >= backgroundTexture.StartingGid &&
								   tile <= backgroundTexture.EndingGid)
								{
									mapTexture = backgroundTexture;
									break;
								}
							}
							
							var sprite = new SpriteUV(mapTexture.TextureInfo);
							
							sprite.Position = new Sce.PlayStation.Core.Vector2(positionX, positionY);
							
							int numberOfTilesAcross = (mapTexture.TextureWidth/mapTexture.TileWidth);
							int numberOfTilesUp = (mapTexture.TextureHeight/mapTexture.TileHeight);
							
							float widthRatioOfTile = 1F/(float)numberOfTilesAcross;
							float heightRatioOfTile = 1F/(float)numberOfTilesUp;
							
							// Grab the x and y of the gid for this tile
							var gidFromStart = tile - mapTexture.StartingGid;
							var x = (gidFromStart % numberOfTilesAcross);
							var y = (gidFromStart / numberOfTilesAcross);
							
							// Position of the window to grab the sprite from
							sprite.UV.T = new Vector2(1F/16F * x, 1F/16F * (numberOfTilesUp - 1 - y));
							
							// Size of the window in relation to the whole texture
							sprite.UV.S = new Vector2(widthRatioOfTile, heightRatioOfTile);
							
							sprite.Quad.S = new Vector2(mapTexture.TextureInfo.TextureSizef.X / numberOfTilesAcross, 
							                            mapTexture.TextureInfo.TextureSizef.Y / numberOfTilesUp);
							
							if(!spritesForLayer.ContainsKey(mapTexture.TextureName))
							{
								spritesForLayer.Add(mapTexture.TextureName, new SpriteList(mapTexture.TextureInfo));
							}
							spritesForLayer[mapTexture.TextureName].AddChild(sprite);
						}
						positionX += map.TileWidth;
					}
					positionY -= map.TileHeight;
				}
				
				// Add each of this layer's spritelists
				foreach(KeyValuePair<string, SpriteList> spriteList in spritesForLayer)
				{
					layer.BackgroundSprites.Add(spriteList.Value);
				}
				
				backgroundLayers.Add(layer);
			}
			
			return backgroundLayers;
		}
		
		private void LoadCollisionLayer(Squared.Tiled.Map map, Squared.Tiled.Layer layer)
		{
			int positionX = 0;
			int positionY = map.Height * map.TileHeight - map.TileHeight;
			int tileCounter = 0;
	
			for(int i = 0; i < map.Height; i++)
			{
				positionX = 0;
				for(int j = 0; j < map.Width; j++)
				{
					var tile = layer.Tiles[tileCounter++];
					
					if(tile > 0)
					{
						CollisionRectangles.Add(new Rectangle(positionX, positionY,
							(float)map.TileWidth, (float)map.TileHeight));
					}				
					positionX += map.TileWidth;
				}
				positionY -= map.TileHeight;
			}
		}
	}
	
	public class BackgroundLayer
	{
		public string LayerName { get; set; }
		public List<SpriteList> BackgroundSprites { get; set; }
		public Vector2 MapSize { get; set; }
	}
	
	public class BackgroundTiledTexture2D
	{
		public int StartingGid { get; set; }
		public int EndingGid {
			get
			{
				return ((TextureWidth / TileWidth) * (TextureHeight / TileHeight)) + (StartingGid - 1);
			}
		}
		public int TileHeight { get; set; }
		public int TileWidth { get; set; }
		public string TextureName { get; set; }
		public Texture2D Texture { get; set; }
		public TextureInfo TextureInfo { get; set; }
		public SpriteList TextureSpriteList { get; set; }
		public int TextureHeight { get { return Texture.Height; } }
		public int TextureWidth { get { return Texture.Width; } }
	}
	
}

