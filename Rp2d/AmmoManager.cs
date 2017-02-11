using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using System.Collections.Generic;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public static class AmmoManager
	{
		public static List<Ammo> Ammo { get; private set; }
		public static ItemPool _pool = new ItemPool();
		
		public static void CreateFireball(Vector2 startingPosition, Vector2 direction)
		{
			if(Ammo == null)
			{
				Ammo = new List<Ammo>();
			}
			
			// Grab Ammo from the pool
			var bullet = (Bullet)_pool.GetItem<Bullet>();
			if(bullet == null)
			{
				bullet = new Bullet(startingPosition, direction);
				_pool.AddItemToPool(bullet);
			}
			else
			{
				bullet.EntitySprite.Position = startingPosition;
				bullet.Direction = direction;
				bullet.Reset();
			}
			
			bullet.IsActive = true;
			Director.Instance.CurrentScene.AddChild(bullet.EntitySprite);
			Ammo.Add(bullet);
			CollisionManager.RegisterCollidableEntity(bullet);
		}
		
		public static void DestroyAmmo(Ammo ammoToDestroy)
		{
			(ammoToDestroy as IPoolable).IsActive = false;
			CollisionManager.CollidableEntities.Remove(ammoToDestroy);
			Director.Instance.CurrentScene.RemoveChild(ammoToDestroy.EntitySprite, true);
			Ammo.Remove(ammoToDestroy);
		}
		
	}
}