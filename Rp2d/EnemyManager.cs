using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using System.Reflection;
using System.Reflection.Emit;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace Rp2d
{
	public static class EnemyManager
	{
		private static List<Enemy> _enemies;
		public static List<Enemy> Enemys { get { return _enemies; } }
		
		private static EnemyPool _enemyPool;
		
		private static Random _random;
		
		private static EnemyRepository _enemyRepository;
		
		// Load the enemy spawn points
		private static SpawnPointsInfo _spawnPointsForMap;
		
		// Load the global enemy repository
		public static void LoadEnemyRepository()
		{
			_enemyRepository = new EnemyRepository();
			
			var enemyRepoEntries = SerializationHelpers.GetXmlNodes<EnemyRepositoryEntry>("/Application/Content/EnemyRepository.xml");			
			_enemyRepository.EnemyRepositoryEntries = new EnemyRepositoryEntry[enemyRepoEntries.Count];
			
			for(int i = 0; i < enemyRepoEntries.Count; i++)
			{
				 _enemyRepository.EnemyRepositoryEntries[i] = 
					SerializationHelpers.ConvertNode<EnemyRepositoryEntry>(enemyRepoEntries[i]);
			}
		}
		
		// Load the WorldMap-specific enemies definition
		public static void LoadMapSpawnPoints(string spawnPointsFilename)
		{
			if(_enemyRepository == null)
			{
				LoadEnemyRepository();
			}
			
			if(!System.IO.File.Exists(spawnPointsFilename))
			{
				return;
			}
			
			_spawnPointsForMap = new SpawnPointsInfo();
			
			var spawnPointList = SerializationHelpers.GetXmlNodes<SpawnPointEntry>(spawnPointsFilename);
			_spawnPointsForMap.SpawnPoints = new SpawnPointEntry[spawnPointList.Count];
			
			for(int i = 0; i < spawnPointList.Count; i++)
			{
				_spawnPointsForMap.SpawnPoints[i] = 
					SerializationHelpers.ConvertNode<SpawnPointEntry>(spawnPointList[i]);
			}
		}
		
		public static void RemoveEnemy(Enemy enemy)
		{
			if(_enemies == null)
			{
				_enemies = new List<Enemy>();
				_enemyPool = new EnemyPool();
			}
			
			CollisionManager.CollidableEntities.Remove(enemy);
			Director.Instance.CurrentScene.RemoveChild(enemy.EntitySprite, true);
			enemy.IsActive = false;
			_enemies.Remove(enemy);
		}
		
		public static void RemoveAllEnemies()
		{
			if(_enemies == null)
			{
				_enemies = new List<Enemy>();
				_enemyPool = new EnemyPool();
			}
			
			// Cleanup the enemy nodes and clear the list
			foreach(var enemy in _enemies)
			{
				enemy.Cleanup();
			}
			_enemies.Clear();
			_enemyPool.ClearPools();
			
			// Cleanup the spawn point definition
			_spawnPointsForMap = new SpawnPointsInfo();
		}
		
		public static void Update(float dt)
		{
			if(_spawnPointsForMap == null || _spawnPointsForMap.SpawnPoints == null)
			{
				return;
			}
			
			// Spawn logic here
			foreach(var spawnPoint in _spawnPointsForMap.SpawnPoints)
			{
				// Check if there is already an enemy in the spawn zone
				int enemiesInSpawnZone = GetNumberOfEnemiesForSpawnPoint(spawnPoint);				
				if(enemiesInSpawnZone < spawnPoint.NumberOfEnemiesAllowedInZone)
				{
					// It is safe to spawn an enemy here
					SpawnEnemyInSpawnPoint(spawnPoint);
				}
			}
		}
		
		private static int GetNumberOfEnemiesForSpawnPoint(SpawnPointEntry spawnPoint)
		{
			if(_enemies == null)
			{
				_enemies = new List<Enemy>();
				_enemyPool = new EnemyPool();
			}
		
			// Enemies have a reference to the spawn point Id that spawned them
			//  Just check the number of enemies in this spawn point
			int enemiesForSpawnZone = 0;
			foreach(var enemy in _enemies)
			{
				if(enemy.SpawnPointId == spawnPoint.Id)
				{
					enemiesForSpawnZone++;
				}
			}
			return enemiesForSpawnZone;
		}
		
		private class ColliderShell : ICollidable
		{
			public Rectangle CollisionRectangle { get { return new Rectangle(); } set { } }
			
			public bool IsStationary { get { return false; } }
		}
		
		private static void SpawnEnemyInSpawnPoint(SpawnPointEntry entry)
		{
			if(_random == null)
			{
				_random = new Random();
			}
			
			// Select a random enemy from the list
			var enemy = GetEnemyFromGlobalRepositoy(
				entry.EnemiesForSpawnPoint[_random.Next(0, entry.EnemiesForSpawnPoint.Length)].EnemyName);
			
			if(enemy == null)
			{
				throw new Exception("Could not find enemy named " + enemy.EnemyName + " in the global repo.");
			}
			
			// TODO: dynamically build additional params
			var additionalParams = new Dictionary<string, object>();
			additionalParams.Add("StartingAnimation", entry.StartingAnimation);
			
			// Create a random start location
			// TODO: Make sure we don't spawn someone in a place that they are already colliding
			float x = -1, y = -1;
			bool alreadyColliding = true;
			while(alreadyColliding)
			{
				x = _random.Next(0, entry.SpawnZoneWidthInPixels) + entry.SpawnZoneStartXInPixels;
				y = _random.Next(0, entry.SpawnZoneHeightInPixels) + entry.SpawnZoneStartYInPixels;
				
				alreadyColliding = CollisionManager.WillCollide(new ColliderShell(), new Rectangle(x, y, 
                	enemy.IndividualTilePixelsX, enemy.IndividualTilePixelsY)).Count > 0;
			}
			
			additionalParams.Add("StartingPositionXInPixels", x);
			additionalParams.Add("StartingPositionYInPixels", y);
				
			// Grab an enemy from the Pool
			var newEnemy = _enemyPool.GetEnemy(enemy, additionalParams);
			if(newEnemy == null) 
			{
				// If there was no available enemy in the pool, create a new one
				newEnemy = EnemyCreator.CreateRoamingEnemy(enemy, additionalParams);
				_enemyPool.AddEnemyToPool(enemy.ClassName, newEnemy);
			}
			else
			{
				newEnemy.EntitySprite.Position = new Vector2(x, y);
				newEnemy.CurrentAnimation = entry.StartingAnimation;
				newEnemy.Reset();
			}
			
			newEnemy.SpawnPointId = entry.Id;
			newEnemy.IsActive = true;
						
			// TODO: it needs to be inserted at the same draw order as NPCs
			Director.Instance.CurrentScene.AddChild(newEnemy.EntitySprite);
			_enemies.Add(newEnemy);
			CollisionManager.CollidableEntities.Add(newEnemy);
		}
		
		private static EnemyRepositoryEntry GetEnemyFromGlobalRepositoy(string enemyName)
		{
			foreach(var enemy in _enemyRepository.EnemyRepositoryEntries)
			{		
				if(enemy.EnemyName.Equals(enemyName))
				{
					return enemy;
				}
			}
			
			return null;
		}
		
	}
}

