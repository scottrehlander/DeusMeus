using System;

namespace Rp2d
{
	public class SpawnPointsInfo
	{
		public SpawnPointEntry[] SpawnPoints { get; set; }
	}
	
	public class SpawnPointEntry
	{		
		public int Id { get; private set; }
		
		public SpawnPointEntry()
		{
			// Auto Id the spawn point zones
			Id = SpawnPointEntry.NextId();
		}
		
		public int SpawnZoneStartXInPixels { get; set; }
		public int SpawnZoneStartYInPixels { get; set; }
		public int SpawnZoneWidthInPixels { get; set; }
		public int SpawnZoneHeightInPixels { get; set; }
		
		public int NumberOfEnemiesAllowedInZone { get; set; }
		public int RespawnRateInMilliseconds { get; set; }
		
		public string StartingAnimation { get; set; }
		public SpawnPointEnemy[] EnemiesForSpawnPoint { get; set; }
		
		
		private static int _idCounter = 0;
		public static int NextId()
		{
			// Don't let the spawn count grow too high...
			//  we will never have more than 2000 spawn points in a zone
			if(_idCounter > 2000)
			{
				_idCounter = 0;
			}
			return _idCounter++;
		}
	}
	
	public class SpawnPointEnemy
	{
		public string EnemyName { get; set; }
	}
}

