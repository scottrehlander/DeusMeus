using System;
using Sce.PlayStation.Core;
using System.Collections.Generic;

namespace Rp2d
{
	public class BlueBird : RoamingEnemy, ICollidable, INotifyOfCollision
	{
		
		Hero _hero;
		
		enum BirdState 
		{
			Roaming,
			Attacking
		};
		private BirdState _birdState;
		
		private string _attackSoundKey = "Squawk1";
		private string _attackSoundFile = "/Application/Content/Audio/Sound Effects/squawk1.wav";
		
		LifeManager _lifeManager = new LifeManager(1);
		
		public BlueBird ()
		{
			InitializeBlueBird();
		}
		
		private void InitializeBlueBird()
		{
			SetCollisionRectangle(22, 16, 5, 0);
		
			_attackSoundKey += Id.ToString();
			AudioManager.PrecacheSound(_attackSoundKey, _attackSoundFile);
			
			_hero = null;
		}
		
		float timeToDelayUntilAttack = .2F;
		float timeToAttackFor = 1F;
		float timeDelayed = 0;
		float timeAttacking = 0;
		float attackSpeedDelta = 3.5F;
		Vector2 attackDirection = Vector2.Zero;
		public override void Update(float dt)
		{
			if(_hero == null)
			{
				_hero = FindHero();
			}
			
			// Update the position of our vision rectangle
			UpdateVisionRectanglePosition(EntitySprite.Position.X - (_visionRectangle.Width / 2) + (_collisionRectangle.Width / 2),
				_visionRectangle.Y = EntitySprite.Position.Y - (_visionRectangle.Height / 2) + (_collisionRectangle.Height / 2));
			
			if(_birdState == BirdState.Roaming && CanSee(_hero.EntitySprite.Position))
			{
				_birdState = BirdState.Attacking;
				
				if(_hero.EntitySprite.Position.X >= EntitySprite.Position.X)
				{
					CurrentAnimation = "walk_right";
				}
				else
				{
					CurrentAnimation = "walk_left";
				}
			}
				
			if(_birdState == BirdState.Attacking)
			{	
				// Delay for a bit before attacking
				if(timeDelayed <= timeToDelayUntilAttack)
				{
					timeDelayed += dt;
				}
				else if(attackDirection == Vector2.Zero)
				{
					// Pick the attack direction
					var xDiff = _hero.EntitySprite.Position.X - EntitySprite.Position.X;
					var yDiff = _hero.EntitySprite.Position.Y - EntitySprite.Position.Y;
					
					// Find the hypotenuse
					// x^2 + y^2 = z^2
					// 
					var hyp = Math.Sqrt((Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2)));
					xDiff = (float)(xDiff / hyp);
					yDiff = (float)(yDiff / hyp);
					attackDirection = new Vector2(xDiff, yDiff);
					
					// Squawk!
					AudioManager.PlayPrecachedSound(_attackSoundKey);
				}
				
				if(timeDelayed > timeToDelayUntilAttack)
				{
					timeAttacking += dt;
					
					// Attack the hero swiftly
					if(_hero != null)
					{
						// SetNewPosition will not set a position if a collision will occur
						SetNewPosition(new Vector2(
							EntitySprite.Position.X + (attackDirection.X * attackSpeedDelta), 
							EntitySprite.Position.Y + (attackDirection.Y * attackSpeedDelta)));
					}
					
					if(timeAttacking > timeToAttackFor)
					{
						timeDelayed = 0;
						timeAttacking = 0;
						attackDirection = Vector2.Zero;
						
						_birdState = BirdState.Roaming;
					}
				}
				
				base.Update(dt, false);
			}
			else
			{
				// Just roam
				base.Update(dt);
			}
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

