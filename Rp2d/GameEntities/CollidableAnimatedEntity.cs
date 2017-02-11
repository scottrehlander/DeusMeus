using System;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public class CollidableAnimatedEntity : AnimatedEntity, ICollidable
	{
		// Define the global parameters for our Collision Rectangle
		protected float COLLISION_RECTANGLE_WIDTH = 22;
		protected float COLLISION_RECTANGLE_HEIGHT = 16;
		protected float COLLISION_RECTANGLE_OFFSET_X = 5;
		protected float COLLISION_RECTANGLE_OFFSET_Y = 0;
	
		public bool IsStationary { get { return false; } }
		
		// Define the collision rectangle for this sprite
		protected Rectangle _collisionRectangle;
		public Rectangle CollisionRectangle 
		{ 
			get 
			{
				_collisionRectangle.X = EntitySprite.Position.X + COLLISION_RECTANGLE_OFFSET_X;
				_collisionRectangle.Y = EntitySprite.Position.Y + COLLISION_RECTANGLE_OFFSET_Y;
				return _collisionRectangle; 
			} 
		}
		
		public CollidableAnimatedEntity ()
		{
		}
		
		protected void SetNewPosition(Vector2 position)
		{
			_previousPosition = EntitySprite.Position;
			if(!position.Equals(EntitySprite.Position))
			{	
				EntitySprite.Position = position;
			}			
		}
		
		protected void SetCollisionRectangle(float width, float height, float offsetX, float offsetY)
		{
			COLLISION_RECTANGLE_WIDTH = width;
			COLLISION_RECTANGLE_HEIGHT = height;
			COLLISION_RECTANGLE_OFFSET_X = offsetX;
			COLLISION_RECTANGLE_OFFSET_Y = offsetY;
			_collisionRectangle = new Rectangle(0, 0, COLLISION_RECTANGLE_WIDTH, COLLISION_RECTANGLE_HEIGHT);
		}
		
	}
}

