using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace Rp2d
{
	public class ArmoredGuard : RoamingEnemy, INotifyOfCollision
	{
		
		enum GuardState 
		{
			Roaming,
			Chasing
		};
		private GuardState _guardState;
		
		Hero _hero;
		
		LifeManager _lifeManager = new LifeManager(3);
		
		public ArmoredGuard ()
		{
			InitializeArmoredGuard();
		}
		
		public ArmoredGuard(bool autoInit) : base(autoInit)
		{
			InitializeArmoredGuard();
		}
		
		public ArmoredGuard(RoamingEnemyInfo enemyInfo) : base(enemyInfo)
 		{
			InitializeArmoredGuard();
		}
		
		private void InitializeArmoredGuard()
		{
			_guardState = GuardState.Roaming;
			
			SetCollisionRectangle(22, 16, 5, 0);
			
			_hero = null;
		}
		
		// Custom movement code
		public override void Update(float dt)
		{			if(_hero == null)
			{
				_hero = FindHero();
			}
			
			// Update the position of our vision rectangle
			UpdateVisionRectanglePosition(EntitySprite.Position.X - (_visionRectangle.Width / 2) + (_collisionRectangle.Width / 2),
				_visionRectangle.Y = EntitySprite.Position.Y - (_visionRectangle.Height / 2) + (_collisionRectangle.Height / 2));
			
			// If we are roaming, default to the base roaming action
			if(_guardState == GuardState.Roaming)
			{
				base.Update(dt);
			}
			else if(_guardState == GuardState.Chasing)
			{
				if(_hero != null)
				{
					// Move in the direction of the hero
					float newPositionXDelta = 1;
					CurrentAnimation = "walk_right";
					if(_hero.EntitySprite.Position.X < EntitySprite.Position.X)
					{
						newPositionXDelta = -1;
						CurrentAnimation = "walk_left";
					}
					float newPositionYDelta = 1;
					if(_hero.EntitySprite.Position.Y < EntitySprite.Position.Y)
					{
						newPositionYDelta = -1;
					}
					
					// SetNewPosition will not set a position if a collision will occur
					SetNewPosition(new Vector2(
						EntitySprite.Position.X + newPositionXDelta, 
						EntitySprite.Position.Y + newPositionYDelta));
				}
				
				// If the player is now outside of the vision of the guard, stop chasing
				if(!CanSee(_hero.EntitySprite.Position))
			  	{
					_guardState = GuardState.Roaming;
				}				
				
			}
			
			// Look for the hero within a certain bounding region (vision) and change state to chasing
			if(CollisionManager.PointIsInRectangle(_visionRectangle, _hero.EntitySprite.Position))
			{
				_guardState = GuardState.Chasing;	
			}
						
			if(_guardState == GuardState.Roaming)
			{
				base.Update(dt);
			}
			else
			{
				base.Update(dt, false);
			}			
		}
		
		public override void Reset()
		{
			_lifeManager.ResetLife();
			base.Reset();
		}
		
		public void NotifyCollision(ICollidable collider)
		{
			if(collider is HeroFiredAmmo)
			{
				_lifeManager.DecreaseLife(1);
			}
			if(_lifeManager.HitPoints <= 0)
			{
				EnemyManager.RemoveEnemy(this);
			}
			
			EntitySprite.Position = _previousPosition;
		}
		
	}
}

