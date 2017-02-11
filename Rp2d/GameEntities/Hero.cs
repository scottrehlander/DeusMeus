using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace Rp2d
{
	public class Hero : AnimatedEntity, ICollidable, INotifyOfCollision
	{
		// Define some basic hero parameters
		private string _heroTexture = "/Application/Content/GameEntities/Char-Scott.png";
		private float _animationFrameSwitchValue = .25F;
		private float _moveAmountDelta = 2.5F;
		
		// Define the global constant parameters for our Collision Rectangle
		private const float COLLISION_RECTANGLE_WIDTH = 22;
		private const float COLLISION_RECTANGLE_HEIGHT = 16;
		private const float COLLISION_RECTANGLE_OFFSET_X = 5;
		private const float COLLISION_RECTANGLE_OFFSET_Y = 0;
		
#if DRAW_COLLISION_RECTS
		private SpriteUV _collisionRectangleSprite;
#endif
		
		public bool IsStationary { get { return false; } }
		
		private Rectangle _collisionRectangle;
		public Rectangle CollisionRectangle 
		{ 
			get 
			{
				_collisionRectangle.X = EntitySprite.Position.X + COLLISION_RECTANGLE_OFFSET_X;
				_collisionRectangle.Y = EntitySprite.Position.Y + COLLISION_RECTANGLE_OFFSET_Y;
				return _collisionRectangle; 
			} 
		}
		private Rectangle _newCollisionRectangleToCheck;
		
		private float _lastBulletFired = 0;
		private bool _actionButtonReset = true;
		
		private LifeManager _lifeManager = new LifeManager(5);
		
		public Hero ()
		{
			_name = "Hero";
			
			InitializeEntity(_heroTexture, new Vector2i(3, 4), new Vector2(32, 32));
			_collisionRectangle = new Rectangle(0, 0, COLLISION_RECTANGLE_WIDTH, COLLISION_RECTANGLE_HEIGHT);
			
			SetAnimation("walk_down", 4, 1, 3, _animationFrameSwitchValue);
			SetAnimation("walk_left", 3, 1, 3, _animationFrameSwitchValue);
			SetAnimation("walk_right", 2, 1, 3, _animationFrameSwitchValue);
			SetAnimation("walk_up", 1, 1, 3, _animationFrameSwitchValue);
			SetAnimation("idle", 4, 2, 2, _animationFrameSwitchValue);
			
			CurrentAnimation = "idle";
			
			EntitySprite = GetEntity();
			
#if DRAW_COLLISION_RECTS
			var collisionRectangleTexture = TextureManager.CreateTexture2D(0x44FFFFFF, (int)_collisionRectangle.Width, (int)_collisionRectangle.Height);
			_collisionRectangleSprite = new SpriteUV(collisionRectangleTexture.Info);
			_collisionRectangleSprite.Quad.S = collisionRectangleTexture.Info.TextureSizef;
			Director.Instance.CurrentScene.AddChild(_collisionRectangleSprite);
#endif
			
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);
		}
		
		protected Rectangle GetCollisionRectangleToCheck(Vector2 newPosition)
		{
			if(_newCollisionRectangleToCheck == null)
			{
				_newCollisionRectangleToCheck = new Rectangle();
			}
			_newCollisionRectangleToCheck.X = newPosition.X + COLLISION_RECTANGLE_OFFSET_X;
			_newCollisionRectangleToCheck.Y = newPosition.Y + COLLISION_RECTANGLE_OFFSET_Y;
			_newCollisionRectangleToCheck.Width = COLLISION_RECTANGLE_WIDTH;
			_newCollisionRectangleToCheck.Height = COLLISION_RECTANGLE_HEIGHT;
			
			return _newCollisionRectangleToCheck;
		}
		
		public override void Update(float dt)
		{
			_lastBulletFired += dt;
			
			if(DialogManager.IsNpcSpeaking())
			{
				return;
			}
			
			var inputAccepted = false;
			
			
			#region Track Hero Action
			
			if(GameInput.GetInputValue(GameInputControls.Action) != 0)
			{
				// The action button was pressed
				if(_actionButtonReset)
				{
					// Check to see if we are facing an NPC
					float positionToCheckX = EntitySprite.Position.X;
					float positionToCheckY = EntitySprite.Position.Y;
					
					if(CurrentAnimation == "walk_right")
					{
						positionToCheckX += _tilePixelDimensions.X;
					}
					else if (CurrentAnimation == "walk_left")
					{
						positionToCheckX -= _tilePixelDimensions.X;
					}
					else if (CurrentAnimation == "walk_down")
					{
						positionToCheckY -= _tilePixelDimensions.Y;
					}
					else if (CurrentAnimation == "walk_up")
					{
						positionToCheckY += _tilePixelDimensions.Y;
					}
					
					_newCollisionRectangleToCheck = GetCollisionRectangleToCheck(
							new Vector2(positionToCheckX, positionToCheckY));
					
					// Check for Npc Collision for the offset box
					var canSpeakTo = CollisionManager.WillCollide(this, _newCollisionRectangleToCheck);
					foreach(var colliable in canSpeakTo)
					{
						if(colliable is Npc)
						{
							DialogManager.SetSpeakingNpc(colliable as Npc);
							return;
						}
					}
					
					// If we get here we aren't speaking to an npc, create a bullet
					if(_lastBulletFired > .1F)
					{
						bool useLastFacingDirection = false;
						
						int xDirection = 0;
						int yDirection = 0;
						if(GameInput.GetInputValue(GameInputControls.MoveRight) != 0)
						{
							xDirection += 1;
						}
						if(GameInput.GetInputValue(GameInputControls.MoveLeft) != 0)
						{
							xDirection -= 1;
						}
						if(GameInput.GetInputValue(GameInputControls.MoveUp) != 0)
						{
							yDirection += 1;
						}
						if(GameInput.GetInputValue(GameInputControls.MoveDown) != 0)
						{
							yDirection -= 1;
						}
						if(xDirection == 0 && yDirection == 0)
						{
							useLastFacingDirection = true;
						}
						
						Vector2 startingPosition;
						if(CurrentAnimation == "walk_right")
						{
							startingPosition = new Vector2(EntitySprite.Position.X + 20, EntitySprite.Position.Y + 10);
							if(useLastFacingDirection)
							{
								xDirection = 1;
							}
						}
						else if(CurrentAnimation == "walk_left")
						{
							startingPosition = new Vector2(EntitySprite.Position.X - 5, EntitySprite.Position.Y + 10);
							if(useLastFacingDirection)
							{
								xDirection = -1;
							}
						}
						else if(CurrentAnimation == "walk_up")
						{
							startingPosition = new Vector2(EntitySprite.Position.X + 13, EntitySprite.Position.Y + 25);
							if(useLastFacingDirection)
							{
								yDirection = 1;
							}
						}
						else
						{
							startingPosition = new Vector2(EntitySprite.Position.X + 13, EntitySprite.Position.Y - 5);
							if(useLastFacingDirection)
							{
								yDirection = -1;
							}
						}
						
						AmmoManager.CreateFireball(startingPosition, new Vector2(xDirection, yDirection));
						_lastBulletFired = 0;
					}
					
					_actionButtonReset = false;
				}
								
				
			}
			else
			{
				_actionButtonReset = true;
			}
			
			#endregion
			
			
			#region Track hero movement
			_previousPosition = EntitySprite.Position;
			float newPositionX = EntitySprite.Position.X;
			float newPositionY = EntitySprite.Position.Y;
			
			if(GameInput.GetInputValue(GameInputControls.MoveRight) != 0)
			{
				CurrentAnimation = "walk_right";
				newPositionX += _moveAmountDelta;
				inputAccepted = true;
				PauseAnimation = false;
			}
			if(GameInput.GetInputValue(GameInputControls.MoveLeft) != 0)
			{
				CurrentAnimation = "walk_left";
				newPositionX -= _moveAmountDelta;
				inputAccepted = true;
				PauseAnimation = false;
			}
			if(GameInput.GetInputValue(GameInputControls.MoveUp) != 0)
			{
				CurrentAnimation = "walk_up";
				newPositionY += _moveAmountDelta;
				inputAccepted = true;
				PauseAnimation = false;
			}
			if(GameInput.GetInputValue(GameInputControls.MoveDown) != 0)
			{
				CurrentAnimation = "walk_down";
				newPositionY -= _moveAmountDelta;
				inputAccepted = true;
				PauseAnimation = false;
			}
			
			if(!inputAccepted)
			{
				PauseAnimation = true;
			}
			
			// Perform the move (assuming no collisions)
			Vector2 newPosition = new Vector2(newPositionX, newPositionY);
			if(!newPosition.Equals(EntitySprite.Position))
			{
				EntitySprite.Position = newPosition;
			}
			
#if DRAW_COLLISION_RECTS
			_collisionRectangleSprite.Position = _collisionRectangle.Position;
#endif
			
			#endregion
			
			base.Update(dt);
		}
		
		public void NotifyCollision(ICollidable collider)
		{ 
			if(collider is Enemy)
			{
				_lifeManager.DecreaseLife(1);
			}
			
			EntitySprite.Position = _previousPosition;
		}
		
	}
}

