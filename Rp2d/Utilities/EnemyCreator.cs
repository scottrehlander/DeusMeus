using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rp2d
{
	public static class EnemyCreator
	{
		
		public static RoamingEnemy CreateRoamingEnemy(EnemyRepositoryEntry repoEntry, Dictionary<string, object> additionalProperties)
		{
			// Enemies must have an empty constructor
			var roamingEnemyBase = (RoamingEnemy)Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, 
                                  repoEntry.ClassName).Unwrap();
			
			var roamingInfo = new RoamingEnemyInfo();
			roamingInfo.AnimationDefinitions = repoEntry.AnimationDefinitions;
			roamingInfo.EnemyName = repoEntry.EnemyName;
			roamingInfo.ImageNumberOfTilesAcross = repoEntry.ImageNumberOfTilesAcross;
			roamingInfo.ImageNumberOfTilesDown = repoEntry.ImageNumberOfTilesDown;
			roamingInfo.IndividualTilePixelsX = repoEntry.IndividualTilePixelsX;
			roamingInfo.IndividualTilePixelsY = repoEntry.IndividualTilePixelsY;
			roamingInfo.TextureFilename = repoEntry.TextureFilename;
			
			roamingInfo.StartingAnimation = additionalProperties["StartingAnimation"].ToString();
			roamingInfo.StartingPositionXInPixels = float.Parse(additionalProperties["StartingPositionXInPixels"].ToString());
			roamingInfo.StartingPositionYInPixels = float.Parse(additionalProperties["StartingPositionYInPixels"].ToString());
			
			roamingEnemyBase.EnemyInfo = roamingInfo;
			roamingEnemyBase.InitializeEnemy();
			
			return roamingEnemyBase;
		}
		
	}
}

