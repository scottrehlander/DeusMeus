using System;
using System.Collections.Generic;

namespace Rp2d
{
	public class EnemyPool
	{
		
		public Dictionary<string, List<Enemy>> _pools = new Dictionary<string, List<Enemy>>();
		
		public Enemy GetEnemy(EnemyRepositoryEntry enemy, Dictionary<string, object> additionalParams)
		{
			// Check the pool to see if there are any available enemies
			List<Enemy> enemiesForThisClass = null;
			if(_pools.ContainsKey(enemy.ClassName))
			{
				enemiesForThisClass = _pools[enemy.ClassName];
			}
			
			if(enemiesForThisClass != null)
			{
				foreach(var poolEnemy in enemiesForThisClass)
				{
					if(poolEnemy.IsActive == false)
					{
						return poolEnemy;
					}
				}
			}
			// Reset the enemy and return it
			//enemy.Reset(additionalParams);
			
			// Return null if there are no enemies available
			return null;
		}
		
		public void AddEnemyToPool(string className, Enemy enemy) 
		{
			if(!_pools.ContainsKey(className))
			{
				_pools.Add(className, new List<Enemy>());
			}
			
			_pools[className].Add(enemy);
		}
		
		public void ClearPools()
		{
			_pools.Clear();
		}
	}
}

