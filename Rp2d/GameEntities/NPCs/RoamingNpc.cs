using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace Rp2d
{
	public class RoamingNpc : Npc
	{
		private RoamingNpcInfo _npcInfo;
		private float _moveAmountDelta = .5F;
		private float _timeToSwitchDirections = 5F;
		private float _timeSinceSwitchedDirection = 0;
		private float _timeStopped = 0;
		private float _timeToStartAgain = 1F;
		private string _lastDirection = "";
		
		private static int _randomSeed = 0;
		public Random _random;
		
		public RoamingNpc()
		{
			// SHOULD PROBABLY JUST REMOVE THIS 
			//  (Will we ever need to call RoamingNpg without ctor data?
			_npcInfo = new RoamingNpcInfo();
			_npcInfo.TextureFilename = "/Application/Content/GameEntities/Char-Christine.png";
			_npcInfo.ImageNumberOfTilesAcross = 3;
			_npcInfo.ImageNumberOfTilesDown = 4;
			_npcInfo.IndividualTilePixelsX = 32;
			_npcInfo.IndividualTilePixelsY = 32;
			
			_npcInfo.AnimationDefinitions = CreateBasicAnimationDefinitions();
			
			_npcInfo.StartingAnimation = "walk_right";
			
			_npcInfo.StartingPositionXInPixels = 600;
			_npcInfo.StartingPositionYInPixels = 1200;
			
			DialogList = _npcInfo.NpcDialogs;
			_currentDialogId = _npcInfo.CurrentDialogId;
			
			InitializeNpc();
		}
		
		public RoamingNpc(RoamingNpcInfo npcInfo)
		{
			_npcInfo = npcInfo;
			
			_currentDialogId = npcInfo.CurrentDialogId;
			DialogList = npcInfo.NpcDialogs;
			
			InitializeNpc();
		}
		
		protected virtual void InitializeNpc()
		{
			_collisionRectangle = new Rectangle(0, 0, 30, 30);
			
			_random = new Random(_randomSeed++);
			_timeToStartAgain = _random.NextFloat() * 3;
			_timeToSwitchDirections = (_random.NextFloat() * 6) + 4F;
			
			// Init the entity
			InitializeEntity(_npcInfo.TextureFilename,
                 new Vector2i(_npcInfo.ImageNumberOfTilesAcross, _npcInfo.ImageNumberOfTilesDown), 
                 new Vector2(_npcInfo.IndividualTilePixelsX, _npcInfo.IndividualTilePixelsY));
			
			// Set the animations
			foreach(AnimationDefinition animation in _npcInfo.AnimationDefinitions)
			{
				SetAnimation(animation.AnimationName, animation.ImageRow, 
				             animation.ImageStartColumn, animation.ImageEndColumn,
				             animation.GameTimeToSwitchFrames);
			}
			
			// Start out walking right
			CurrentAnimation = _npcInfo.StartingAnimation;
			
			// Initialize the sprite and set the initial position
			EntitySprite = GetEntity();
			EntitySprite.Position = new Vector2(
				_npcInfo.StartingPositionXInPixels, _npcInfo.StartingPositionYInPixels);
			
			// Schedule the Update function to be called by Director
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);
		}
		
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
			
			// CONSOLIDATE THIS CODE?
			// Check if moving x would cause us to collide
			bool xFailed = false;
			bool yFailed = false;
			Vector2 newPosition = new Vector2(newPositionX, newPositionY);
			if(!newPosition.Equals(Position))
			{
				// Check collision in X
				float offset = CurrentAnimation == "walk_left" ? -10 : 10;
				newPosition = new Vector2(newPositionX + offset, EntitySprite.Position.Y);
				var willCollide = CollisionManager.WillCollide(this, GetCollisionRectangleToCheck(newPosition));
				if(willCollide.Count > 0)
				{
					// See if we colided with the hero
					Hero hero = null;
					foreach(var collider in willCollide)
					{
						if(collider is Hero)
						{
							hero = collider as Hero;
							break;
						}
					}
					
					// If so, then face him
					if(hero != null)
					{
						if(hero.EntitySprite.Position.X > EntitySprite.Position.X)
						{
							CurrentAnimation = "walk_right";
						}
						else
						{
							CurrentAnimation = "walk_left";
						}
					}
					
					// X failed
					xFailed = true;
				}
				newPosition = new Vector2(EntitySprite.Position.X, newPositionY);
				if(CollisionManager.WillCollide(this, GetCollisionRectangleToCheck(newPosition)).Count > 0)
				{
					// Y failed
					yFailed = true;
				}
				
				if(xFailed || yFailed)
				{
					PauseAnimation = true;
				}
				else
				{
					PauseAnimation = false;
					
					// Based on our tests, set the new position
					newPosition = new Vector2(
						xFailed ? EntitySprite.Position.X : newPositionX, 
						yFailed ? EntitySprite.Position.Y : newPositionY);
					
					EntitySprite.Position = newPosition;
				}
			}
			
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
		
		protected Rectangle GetCollisionRectangleToCheck(Vector2 newPosition)
		{
			if(_newCollisionRectangleToCheck == null)
			{
				_newCollisionRectangleToCheck = new Rectangle();
			}
			_newCollisionRectangleToCheck.X = newPosition.X - 30;
			_newCollisionRectangleToCheck.Y = newPosition.Y - 30;
			_newCollisionRectangleToCheck.Width = _collisionRectangle.Width + 60;
			_newCollisionRectangleToCheck.Height = _collisionRectangle.Height + 60;
			
			return _newCollisionRectangleToCheck;
			
		}
	}
}

