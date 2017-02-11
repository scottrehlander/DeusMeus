using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public class RoamingEnemy : Enemy
	{
		
		private RoamingEnemyInfo _enemyInfo;
		public RoamingEnemyInfo EnemyInfo { get { return _enemyInfo; } set { _enemyInfo = value; } }
		
		private float _moveAmountDelta = .5F;
		
		private static int _randomSeed = 0;
		public Random _random;
		
		// Create a rectangle that defines the enemy's vision, defaut is 250px vision
		protected Rectangle _visionRectangle = new Rectangle(0, 0, 250, 250);
			
#if DRAW_COLLISION_RECTS
		private SpriteUV _collisionRectangleSprite;		
		protected SpriteUV _visionRectangleSprite;		
#endif

		public RoamingEnemy()
		{
		}
		
		public RoamingEnemy(bool autoInit)
		{
			_enemyInfo = new RoamingEnemyInfo();
			_enemyInfo.TextureFilename = "/Application/UnitTests/TestFiles/ArmoredGuard.png";
			_enemyInfo.ImageNumberOfTilesAcross = 3;
			_enemyInfo.ImageNumberOfTilesDown = 4;
			_enemyInfo.IndividualTilePixelsX = 32;
			_enemyInfo.IndividualTilePixelsY = 32;
			
			_enemyInfo.AnimationDefinitions = CreateBasicAnimationDefinitions();
			_enemyInfo.StartingAnimation = "walk_right";
			
			_enemyInfo.StartingPositionXInPixels = 500;
			_enemyInfo.StartingPositionYInPixels = 200;
			
			if(autoInit)
			{
				InitializeEnemy();
			}
		}
		
		public RoamingEnemy(RoamingEnemyInfo enemyInfo)
		{
			_enemyInfo = enemyInfo;
						
			InitializeEnemy();
		}
		
		public void InitializeEnemy()
		{
			
#if DRAW_COLLISION_RECTS
			var visionRectangleTexture = TextureManager.CreateTexture2D(0x4444FFFF, (int)_visionRectangle.Width, (int)_visionRectangle.Height);
			_visionRectangleSprite = new SpriteUV(visionRectangleTexture.Info);
			_visionRectangleSprite.Quad.S = visionRectangleTexture.Info.TextureSizef;
			Director.Instance.CurrentScene.AddChild(_visionRectangleSprite);

			var collisionRectangleTexture = TextureManager.CreateTexture2D(0x44FFFFFF, (int)_collisionRectangle.Width, (int)_collisionRectangle.Height);
			_collisionRectangleSprite = new SpriteUV(collisionRectangleTexture.Info);
			_collisionRectangleSprite.Quad.S = collisionRectangleTexture.Info.TextureSizef;
			Director.Instance.CurrentScene.AddChild(_collisionRectangleSprite);
#endif
			
			// Init the entity
			InitializeEntity(_enemyInfo.TextureFilename,
                 new Vector2i(_enemyInfo.ImageNumberOfTilesAcross, _enemyInfo.ImageNumberOfTilesDown), 
                 new Vector2(_enemyInfo.IndividualTilePixelsX, _enemyInfo.IndividualTilePixelsY));
			
			// Set the animations
			foreach(AnimationDefinition animation in _enemyInfo.AnimationDefinitions)
			{
				SetAnimation(animation.AnimationName, animation.ImageRow, 
				             animation.ImageStartColumn, animation.ImageEndColumn,
				             animation.GameTimeToSwitchFrames);
			}
			
			// Set the current animation
			CurrentAnimation = "walk_right";
			
			// Set EntitySprite
			EntitySprite = GetEntity();
			EntitySprite.Position = new Vector2(
				_enemyInfo.StartingPositionXInPixels, _enemyInfo.StartingPositionYInPixels);
			
			_random = new Random(_randomSeed++);
			
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);
		}
	
		
		// TODO: Make a shared algorithm for roaming entities
		//       Rework enemy AI to aggro hero and change speed
		
		// IF WE CHANGE THESE VARIABLES, CHANGE Reset() also
		float _timeStopped = 0;
		float _timeToStartAgain = 1F;
		float _timeSinceSwitchedDirection = 0;
		float _timeToSwitchDirections = 5F;
		string _lastDirection = "";
		public override void Update(float dt)
		{
			// If we are stopped, don't walk left or right
			if(_timeStopped > 0)
			{
				_timeStopped += dt;
				CurrentAnimation = "idle";
			}
			if(_timeStopped > _timeToStartAgain)
			{
				// If we have been stopped for too long then turn around
				if(_lastDirection.Equals("walk_left"))
				{
					CurrentAnimation = "walk_left";
				}
				else
				{
					CurrentAnimation = "walk_right";
				}
				
				_timeStopped = 0;
				_timeToStartAgain = _random.NextFloat() * 3;
			}
			
			// If we are not idle, then let's walk left or right
			if(CurrentAnimation != "idle")
			{
				_timeSinceSwitchedDirection += dt;
			
				// Check to see if we've been walking in one direction too long
				if(_timeSinceSwitchedDirection > _timeToSwitchDirections)
				{
					SwitchDirections();
				}
			}
			
			float newPositionX = EntitySprite.Position.X;
			float newPositionY = EntitySprite.Position.Y;
			
			if(CurrentAnimation.Equals("walk_left"))
			{
				_lastDirection = "walk_left";
				newPositionX -= _moveAmountDelta;
			}
			else if(CurrentAnimation.Equals("walk_right"))
			{
				_lastDirection = "walk_right";
				newPositionX += _moveAmountDelta;
			}
			
			Vector2 newPosition = new Vector2(newPositionX, newPositionY);
			SetNewPosition(newPosition);
			
#if DRAW_COLLISION_RECTS			
			_visionRectangleSprite.Position = _visionRectangle.Position;
			_collisionRectangleSprite.Position = _collisionRectangle.Position;
#endif			
			
			base.Update(dt);
		}
		
		protected void Update(float dt, bool isRoaming)
		{
#if DRAW_COLLISION_RECTS
			_visionRectangleSprite.Position = _visionRectangle.Position;
			_collisionRectangleSprite.Position = _collisionRectangle.Position;
#endif			
			base.Update(dt);
		}
		
		private void SwitchDirections()
		{
			// If we have, let's stop for a bit
			if(CurrentAnimation.Equals("walk_left"))
			{
				CurrentAnimation = "walk_right";
				_timeStopped = .01F;
			}
			else
			{
				CurrentAnimation = "walk_left";
				_timeStopped = .01F;
			}
			
			// Random a new distance to walk next time
			_timeSinceSwitchedDirection = 0;
			_timeToSwitchDirections = (_random.NextFloat() * 6) + 4F;
		}
		
		protected void UpdateVisionRectanglePosition(float x, float y)
		{
			// Update the position of our vision rectangle
			_visionRectangle.X = x;
			_visionRectangle.Y = y;
		}
		
		protected bool CanSee(Vector2 positionOfThingToSee)
		{
			return CollisionManager.PointIsInRectangle(_visionRectangle, positionOfThingToSee);
		}
		
		public override void Reset()
		{
			_timeStopped = 0;
			_timeToStartAgain = 1F;
			_timeSinceSwitchedDirection = 0;
			_timeToSwitchDirections = 5F;
			_lastDirection = "";
		}
	}
}

