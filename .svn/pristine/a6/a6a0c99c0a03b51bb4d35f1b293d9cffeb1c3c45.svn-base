using System;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public class EmptyCollisionEntity : ICollidable
	{
		private Rectangle _collisionRectangle;
		public Rectangle CollisionRectangle { get { return _collisionRectangle; } }
		
		public bool IsStationary { get { return true; } }
		
		public EmptyCollisionEntity(Rectangle collisionRectangle)
		{
			_collisionRectangle = collisionRectangle;
		}
	}
}

