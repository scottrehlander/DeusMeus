using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace Rp2d
{
	public class Bullet : HeroFiredAmmo, INotifyOfCollision
	{
		
		public Vector2 Direction { get; set; }
		
		float _speed = 5;
		float _maxDistance = 300;
		float _distanceTraveled = 0;
		
		public Bullet (Vector2 startingPosition, Vector2 direction)
		{
			Direction = direction;
			
			InitializeEntity("/Application/Content/GameEntities/Ammo/fireball.png", 
                 new Vector2i(1, 1),
                 new Vector2(10, 10));
			
			SetAnimation("firing", 1, 1, 1, 1F);
			
			CurrentAnimation = "firing";
			
			EntitySprite = GetEntity();
			
			EntitySprite.Position = new Vector2(
				startingPosition.X, startingPosition.Y);
			
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);
			
			InitializeBullet();
		}
		
		private void InitializeBullet()
		{
			SetCollisionRectangle(10, 10, 2.5F, 2.5F);
		}
		
		public override void Update(float dt)
		{
			if(!IsActive)
			{
				return;
			}
			
			_distanceTraveled += _speed;
			if(_distanceTraveled > _maxDistance)
			{
				AmmoManager.DestroyAmmo(this);
			}
			else
			{
				SetNewPosition(new Vector2(EntitySprite.Position.X + (Direction.X * _speed), EntitySprite.Position.Y + (Direction.Y * _speed)));
			}
		}
		
		public override void Reset()
		{
			_distanceTraveled = 0;
		}
			
		public void NotifyCollision(ICollidable collider)
		{
			if(collider is Enemy)
			{
				// If we collided with an enemy, remove it
				AmmoManager.DestroyAmmo(this);
			}
		}
		
	}
	
}
