using System;
using VitaUnit;
using System.Linq;

namespace Rp2d
{
	[TestClass]
	public class SerializationTests
	{
		public SerializationTests ()
		{
		}
		
		public void Run()
		{
			TestLoadEnemyRepo();
			TestLoadNpcXml();
			TestLoadWorldMapXml();
			TestLoadMapSpawnPoints();
		}
		
		[TestMethod]
		private void TestLoadEnemyRepo()
		{
			var enemyRepo = SerializationHelpers.GetXmlNodes<EnemyRepositoryEntry>("/Application/UnitTests/TestFiles/EnemyRepository.xml");
			RunTests.AreEqual(1, enemyRepo.Count);
			
			var enemyEntry = SerializationHelpers.ConvertNode<EnemyRepositoryEntry>(enemyRepo[0]);
			RunTests.AreEqual<string>("Black Armored Guard", enemyEntry.EnemyName);
			
			RunTests.AreEqual(5, enemyEntry.AnimationDefinitions.Length);
			
			// Let's go ahead and do a run through of the enemy manager load repo
			EnemyManager.LoadEnemyRepository();
		}
		
		[TestMethod]
		private void TestLoadNpcXml()
		{
			var npcNodeList = SerializationHelpers.GetXmlNodes<RoamingNpcInfo>("/Application/UnitTests/TestFiles/Map1.npc");
			RunTests.AreEqual(3, npcNodeList.Count);
			
			var	npcInfo = SerializationHelpers.ConvertNode<RoamingNpcInfo>(npcNodeList[0]);
			RunTests.AreEqual<string>("Scott", npcInfo.NpcName);
			RunTests.AreEqual(3, npcInfo.ImageNumberOfTilesAcross);
			RunTests.AreEqual<string>("walk_right", npcInfo.StartingAnimation);
			
			// Test the animation info
			RunTests.AreEqual(5, npcInfo.AnimationDefinitions.Length);
			var idle = npcInfo.AnimationDefinitions[0];
			RunTests.AreEqual<string>("idle", idle.AnimationName);
			RunTests.AreEqual(2, idle.ImageStartColumn);
			RunTests.AreEqual(2, idle.ImageEndColumn);

			var walkDown = npcInfo.AnimationDefinitions[1];
			RunTests.AreEqual<string>("walk_up", walkDown.AnimationName);
			RunTests.AreEqual(1, walkDown.ImageStartColumn);
			RunTests.AreEqual(3, walkDown.ImageEndColumn);
			
			// Let's try to add this NPC to the test scene
			RoamingNpc npc = new RoamingNpc(npcInfo);
			
			Game.Instance.Scene.AddChild(npc.EntitySprite);
		}
		
		[TestMethod]
		private void TestLoadWorldMapXml()
		{
			var worldMapNodeList = SerializationHelpers.GetXmlNodes<WorldMapDefinition>("/Application/UnitTests/TestFiles/WorldMap.xml");
			RunTests.AreEqual(2, worldMapNodeList.Count);
			
			var	worldMapDefinition = SerializationHelpers.ConvertNode<WorldMapDefinition>(worldMapNodeList[0]);
			RunTests.AreEqual<string>("Deuston", worldMapDefinition.MapName);
			RunTests.AreEqual(1, worldMapDefinition.WorldMapExitAreas.Length);
			RunTests.AreEqual(2, worldMapDefinition.WorldMapExitAreas[0].MapToExitTo);
			RunTests.AreEqual("/Application/TestFiles/bg_music1.mp3", worldMapDefinition.MapBackgroundSongFilename);
			
			worldMapDefinition = SerializationHelpers.ConvertNode<WorldMapDefinition>(worldMapNodeList[1]);
			RunTests.AreEqual<string>("Castle Deuston", worldMapDefinition.MapName);
			RunTests.AreEqual(1, worldMapDefinition.WorldMapExitAreas.Length);
			RunTests.AreEqual(1, worldMapDefinition.WorldMapExitAreas[0].MapToExitTo);
			RunTests.AreEqual("/Application/TestFiles/bg_music2.mp3", worldMapDefinition.MapBackgroundSongFilename);
		}
		
		[TestMethod]
		private void TestLoadMapSpawnPoints()
		{
			var spawnPointList = SerializationHelpers.GetXmlNodes<SpawnPointEntry>("/Application/UnitTests/TestFiles/Map1.spawns");
			RunTests.AreEqual(1, spawnPointList.Count);
			
			var spawnPoint = SerializationHelpers.ConvertNode<SpawnPointEntry>(spawnPointList[0]);
			RunTests.AreEqual(0, spawnPoint.SpawnZoneStartXInPixels);
			RunTests.AreEqual(100, spawnPoint.SpawnZoneWidthInPixels);
			
			RunTests.AreEqual(1, spawnPoint.EnemiesForSpawnPoint.Length);
			RunTests.AreEqual<string>("Black Armored Guard", spawnPoint.EnemiesForSpawnPoint[0].EnemyName);
		}
		
	}
}

