using System;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using VitaUnit;
using System.Collections.Generic;

namespace Rp2d
{
	
	[TestClass]
	public class EntityTests
	{
		
		public void Run()
		{
			TestAnimatedEntity();
			CreateEnemy();
			CreateEnemyFromRepo();
		}
				
		[TestMethod]
		public void TestAnimatedEntity()
		{
			TestEntity entity = new TestEntity();
		}
		
		[TestMethod]
		public void CreateEnemy()
		{
			RoamingEnemy enemy = new RoamingEnemy(true);
			Game.Instance.Scene.AddChild(enemy.EntitySprite);
		}
		
		[TestMethod]
		public void CreateEnemyFromRepo()
		{
			EnemyRepositoryEntry repoEntry = new EnemyRepositoryEntry();
			
			repoEntry.EnemyName = "Dynamic Guard";
			repoEntry.ClassName = "Rp2d.ArmoredGuard";
			repoEntry.TextureFilename = "/Application/UnitTests/TestFiles/ArmoredGuard.png";
			repoEntry.ImageNumberOfTilesAcross = 3;
			repoEntry.ImageNumberOfTilesDown = 4;
			repoEntry.IndividualTilePixelsX = 32;
			repoEntry.IndividualTilePixelsY = 32;
			
			Dictionary<string, object> additionalProps = new Dictionary<string, object>();
			additionalProps.Add("StartingAnimation", "walk_right");
			additionalProps.Add("StartingPositionXInPixels", 100);
			additionalProps.Add("StartingPositionYInPixels", 300);
			
			List<AnimationDefinition> animations = new List<AnimationDefinition>();
			animations.Add(new AnimationDefinition() {
				AnimationName = "walk_down",
				GameTimeToSwitchFrames = .2F,
				ImageStartColumn = 1,
				ImageEndColumn = 3,
				ImageRow = 4
			});
			animations.Add(new AnimationDefinition() {
				AnimationName = "walk_left",
				GameTimeToSwitchFrames = .2F,
				ImageStartColumn = 1,
				ImageEndColumn = 3,
				ImageRow = 3
			});
			animations.Add(new AnimationDefinition() {
				AnimationName = "walk_right",
				GameTimeToSwitchFrames = .2F,
				ImageStartColumn = 1,
				ImageEndColumn = 3,
				ImageRow = 2
			});
			animations.Add(new AnimationDefinition() {
				AnimationName = "walk_up",
				GameTimeToSwitchFrames = .2F,
				ImageStartColumn = 1,
				ImageEndColumn = 3,
				ImageRow = 1
			});
			animations.Add(new AnimationDefinition() {
				AnimationName = "idle",
				GameTimeToSwitchFrames = .2F,
				ImageStartColumn = 2,
				ImageEndColumn = 2,
				ImageRow = 4
			});
			repoEntry.AnimationDefinitions = animations.ToArray();
			
			var enemy = EnemyCreator.CreateRoamingEnemy(repoEntry, additionalProps);
			Game.Instance.Scene.AddChild(enemy.EntitySprite);
		}
	}
	
	public class TestEntity : AnimatedEntity
	{
		private bool _firstFrame = true;
		private float _totalTimeElapsed = 0;
		private float _animationFrameSwitchValue = .25F;
		
		public TestEntity ()
		{	
			_name = "Test";
			
			InitializeEntity("/Application/Content/GameEntities/Char-Costas.png",
			                 new Vector2i(3, 4), new Vector2(32, 32));
			
			SetAnimation("walk_down", 4, 1, 3, _animationFrameSwitchValue);
			SetAnimation("walk_left", 3, 1, 3, _animationFrameSwitchValue);
			SetAnimation("walk_right", 2, 1, 3, _animationFrameSwitchValue);
			SetAnimation("walk_up", 1, 1, 3, _animationFrameSwitchValue);
			SetAnimation("idle", 4, 2, 2, _animationFrameSwitchValue);
			
			CurrentAnimation = "idle";
			
			EntitySprite = GetEntity();
			
			Game.Instance.Scene.AddChild(EntitySprite);
			
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);
		}
		
		private float _switchToNewAnimation = 0;
		public override void Update(float dt)
		{
			EntitySprite.Position = Director.Instance.CurrentScene.Camera2D.Center;
			
			_totalTimeElapsed += dt;
			_switchToNewAnimation += dt;
			if(_switchToNewAnimation > 2)
			{
				if(CurrentAnimation.Equals("walk_right"))
				{
					CurrentAnimation = "walk_up";
				}
				else if(CurrentAnimation.Equals("walk_up"))
				{
					CurrentAnimation = "walk_left";
				}
				else if(CurrentAnimation.Equals("walk_left"))
				{
					CurrentAnimation = "walk_down";
				}
				else
				{
					CurrentAnimation = "walk_right";
				}
				_switchToNewAnimation = 0;
			}
			
			
			if(_firstFrame)
			{
				RunFirstFrameTests();
				_firstFrame = false;
			}
			
			base.Update(dt);
			
			// A full cycle will be _animationFrameSwitchValue * 3
			//var animationFrameElapsed = _totalTimeElapsed % (_animationFrameSwitchValue * 3);
			if(AnimationTimeElapsed < _animationFrameSwitchValue)
			{
				RunTests.AreEqual(0, CurrentFrameNumber);
			}
			else if(AnimationTimeElapsed < _animationFrameSwitchValue * 2)
			{
				RunTests.AreEqual(1, CurrentFrameNumber);
			}
			else if(AnimationTimeElapsed < _animationFrameSwitchValue * 3)
			{
				RunTests.AreEqual(2, CurrentFrameNumber);
			}
		}
		
		public void RunFirstFrameTests()
		{
			var exceptionThrown = false;
			try
			{
				CurrentAnimation = "AnimationThatDoesn'tExist";
			}
			catch
			{
				exceptionThrown = true;	
			}
			if(!exceptionThrown) { RunTests.TestFailed("Animation does not exists should have been thrown"); }
			
			CurrentAnimation = "walk_right";
			
		}
		
	}
}

