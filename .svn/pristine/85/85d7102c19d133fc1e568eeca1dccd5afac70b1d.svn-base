using System;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace Rp2d
{
	public class EntityBase : Node
	{
		public EntityBase ()
		{
			RegisterDisposeOnExitRecursive();
		}
		
		protected Hero FindHero()
		{
			Hero hero = null;
			foreach(var collider in CollisionManager.CollidableEntities)
			{
				if(collider is Hero)
				{
					hero = collider as Hero;
				}
			}
			return hero;
		}
	}
}

