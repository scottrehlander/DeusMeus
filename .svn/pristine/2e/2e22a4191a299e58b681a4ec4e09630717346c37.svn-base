using System;
using VitaUnit;
using System.Collections.Generic;

namespace Rp2d
{
	[TestClass]
	public class AudioTests
	{
		public void Run()
		{
			//TestSoundEffect();
			//TestBackgroundSong();
		}
		
		[TestMethod]
		public void TestSoundEffect()
		{
			AudioManager.PrecacheSound("/Application/UnitTests/TestFiles/alarm.wav", "/Application/UnitTests/TestFiles/alarm.wav");
			AudioManager.PlayPrecachedSound("/Application/UnitTests/TestFiles/alarm.wav");
		}
		
		[TestMethod]	
		public void TestBackgroundSong()
		{
			AudioManager.PlayBackgroundSong("/Application/UnitTests/TestFiles/bg_music1.mp3");
		}
	}
}

