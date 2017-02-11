using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;

namespace Rp2d
{
	
	public interface ICollidable
	{
		Rectangle CollisionRectangle { get; }
		bool IsStationary { get; }
	}
	
	public interface INotifyOfCollision
	{
		void NotifyCollision(ICollidable collider);
	}
	
	public static class CollisionManager
	{
		private static List<ICollidable> _collidableEntites;
		public static List<ICollidable> CollidableEntities { get { return _collidableEntites; } }
		
		private static List<ICollidable> _willCollideWith;
		
		public static List<ICollidable> WillCollide(ICollidable entity, Rectangle collisionRectangleToCheck)
		{
			// Initialize the list to return
			if(_willCollideWith == null)
			{
				_willCollideWith = new List<ICollidable>();
			}
			
			// Clear the collide with collection
			_willCollideWith.Clear();
			
			// Check for collisions
			// Consider trying this check with one pass through the collidable but with
			//  collision rectangles in both dimensions to check if x fails or y fails
			var enemiesToRemove_REMOVETHISCODE = new List<Enemy>();
			if(_collidableEntites != null)
			{
				foreach(var checkEntity in _collidableEntites)
				{
					if(checkEntity == entity)
					{
						continue;
					}
					
					// Hero and Hero's Ammo shouldn't collide
					if((checkEntity is Hero && entity is HeroFiredAmmo) ||
					   (entity is Hero && checkEntity is HeroFiredAmmo))
					{
						continue;
					}
					
				 	if(RectanglesIntersect(checkEntity.CollisionRectangle, collisionRectangleToCheck))
					{
						// TODO REMOVE THIS CODE ITS JUST FOR FUN
						if(entity is Ammo && checkEntity is Enemy)
						{
							enemiesToRemove_REMOVETHISCODE.Add(checkEntity as Enemy);
						}
						
						_willCollideWith.Add(checkEntity);
					}
				}
			}
			
			foreach(var enemy in enemiesToRemove_REMOVETHISCODE)
			{
				EnemyManager.RemoveEnemy(enemy);
			}
			
			return _willCollideWith;
		}
		
		public static void RegisterCollidableEntity(ICollidable entity)
		{
			if(_collidableEntites == null)
			{
				_collidableEntites = new List<ICollidable>();
			}
			
			_collidableEntites.Add(entity);
		}
		
		public static void RemoveAll()
		{
			if(_collidableEntites == null)
			{
				_collidableEntites = new List<ICollidable>();
			}
			
			_collidableEntites.Clear();
		}
		
		public static bool RectanglesIntersect(Rectangle a, Rectangle b)
		{
			if (
				// Check that right-most part of rect 2 is to the right of rect 1
				a.X < b.X + b.Width && 
				// Check that the right-most part of rect 1 is to the right of rect 2
			    a.X + a.Width > b.X &&
				// Check that rect 1's bottom is under the top-most part of rect 2
    			a.Y < b.Y + b.Height && 
				// Check that rect 1's top-most part is higher than rect 2's bottom
			    a.Y + a.Height > b.Y) 
			{
				return true;
			}
			
			return false;
		}
		
		public static bool PointIsInRectangle(Rectangle rect, Vector2 point)
		{
			if(point.X > rect.X && point.X < rect.X + rect.Width &&
			   point.Y > rect.Y && point.Y < rect.Y + rect.Height)
			{
				return true;
			}
			return false;
		}
		
		private static List<ICollidable> tempColliablesList = new List<ICollidable>();
		private static bool alreadyNotified = false;
		public static void NotifyCollisions(float dt)
		{
			// Populate a temporary list of collidables to make it safe for
			//  notification handlers to deregister colliders
			tempColliablesList.Clear();
			foreach(var collider in _collidableEntites)
			{
				tempColliablesList.Add(collider);
			}
			
			// Associative arrays to keep track of the objects we've already notified
			List<ICollidable> alreadyCollided = new List<ICollidable>();
			List<ICollidable> collidedWith = new List<ICollidable>();
			
			if(_collidableEntites != null)
			{
				foreach(var checkEntity in tempColliablesList)
				{
					foreach(var checkEntity2 in tempColliablesList)
					{
						if(checkEntity.IsStationary && checkEntity2.IsStationary)
						{
							continue;
						}
						
						// Make sure at least one of the two accepts notifictions
						if(!(checkEntity is INotifyOfCollision) && !(checkEntity2 is INotifyOfCollision))
						{
							continue;
						}
						// Don't check an entity against itself
						if(checkEntity == checkEntity2)
						{
							continue;
						}
						alreadyNotified = false;
						for(int i = 0; i < alreadyCollided.Count; i++)
						{
							if((checkEntity == alreadyCollided[i] && checkEntity2 == collidedWith[i]) ||
							   (checkEntity2 == alreadyCollided[i] && checkEntity == collidedWith[i]))
							{
								// Already notified
								alreadyNotified = true;
								break;
							}
						}
						if(alreadyNotified)
						{
							continue;
						}
						
					 	if(RectanglesIntersect(checkEntity.CollisionRectangle, checkEntity2.CollisionRectangle))
						{
							if(checkEntity is INotifyOfCollision)
							{
								(checkEntity as INotifyOfCollision).NotifyCollision(checkEntity2);
							}
							if(checkEntity2 is INotifyOfCollision)
							{
								(checkEntity2 as INotifyOfCollision).NotifyCollision(checkEntity);
							}
							
							alreadyCollided.Add(checkEntity);
							collidedWith.Add(checkEntity2);
						}
					}
				}
			}
		}
		
	}
	
//	public class CollisionHelper
//	{
//		
//		AnimatedEntity _entity;
//		Rectangle _newCollisionRectangleToCheck;
//		
//		public CollisionHelper()
//		{
//		}
//
//		public Vector2 ApplyCollisions(Vector2 newPosition)
//		{
//			// Check if moving x would cause us to collide
//			bool xFailed = false;
//			bool yFailed = false;
//			if(!newPosition.Equals(_entity.EntitySprite.Position))
//			{
//				if(CollisionManager.WillCollide(this as ICollidable, GetCollisionRectangleToCheck(newPosition)).Count > 0)
//				{
//					// X failed
//					xFailed = true;
//				}
//				// TODO: keep track of _newPosition vs newPosition...?
//				newPosition = new Vector2(_entity.EntitySprite.Position.X, _newPosition.Y);
//				if(CollisionManager.WillCollide(this as ICollidable, GetCollisionRectangleToCheck(newPosition)).Count > 0)
//				{
//					// Y failed
//					yFailed = true;
//				}
//				
//				// Based on our tests, set the new position
//				return new Vector2(
//					xFailed ? _entity.EntitySprite.Position.X : _newPosition.X, 
//					yFailed ? _entity.EntitySprite.Position.Y : _newPosition.Y);
//			}
//		}
//		
//		private Rectangle GetCollisionRectangleToCheck(Vector2 newPosition)
//		{
//			if(_newCollisionRectangleToCheck == null)
//			{
//				_newCollisionRectangleToCheck = new Rectangle();
//			}
//			_newCollisionRectangleToCheck.X = newPosition.X + COLLISION_RECTANGLE_OFFSET_X;
//			_newCollisionRectangleToCheck.Y = newPosition.Y + COLLISION_RECTANGLE_OFFSET_Y;
//			_newCollisionRectangleToCheck.Width = COLLISION_RECTANGLE_WIDTH;
//			_newCollisionRectangleToCheck.Height = COLLISION_RECTANGLE_HEIGHT;
//			
//			return _newCollisionRectangleToCheck;
//		}
//		
//	}
}