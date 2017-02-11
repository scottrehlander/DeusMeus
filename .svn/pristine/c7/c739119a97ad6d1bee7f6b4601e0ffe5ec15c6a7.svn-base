using System;

namespace Rp2d
{
	public class LifeManager
	{
		
		private int _startingHitPoints;
		public int HitPoints { get; private set; }
		
		public LifeManager (int hitPoints)
		{
			_startingHitPoints = hitPoints;
			ResetLife();
		}
		
		public void DecreaseLife(int attackAmount)
		{
			HitPoints -= attackAmount;
		}
		
		public void ResetLife()
		{
			HitPoints = _startingHitPoints;	
		}
		
	}
}

