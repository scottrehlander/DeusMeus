using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace Rp2d
{
	
	public interface IPoolable
	{
		bool IsActive { get; set; }
	}
	
	public class ItemPool
	{
		
		public Dictionary<string, List<IPoolable>> _pools = new Dictionary<string, List<IPoolable>>();
		
		public IPoolable GetItem<T>()
		{
			var itemType = typeof(T).ToString();
			
			// Check the pool to see if there are any available enemies
			List<IPoolable> itemsForThisKey = null;
			if(_pools.ContainsKey(itemType))
			{
				itemsForThisKey = _pools[itemType];
			}
			
			if(itemsForThisKey != null)
			{
				foreach(var item in itemsForThisKey)
				{
					if(item.IsActive == false)
					{
						return item;
					}
				}
			}
			
			// Return null if there are no items available
			return null;
		}
		
		public void AddItemToPool(IPoolable item) 
		{
			var itemType = item.GetType().ToString();
			if(!_pools.ContainsKey(itemType))
			{
				_pools.Add(itemType, new List<IPoolable>());
			}
			
			_pools[itemType].Add(item);
		}
		
		public void ClearPools()
		{
			_pools.Clear();
		}
		
	}
}

