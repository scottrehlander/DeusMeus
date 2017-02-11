using System;

namespace Rp2d
{
	public class AnimationDefinition
	{
		public string AnimationName { get; set; }
		public int ImageRow { get; set; }
		public int ImageStartColumn { get; set; }
		public int ImageEndColumn { get; set; }
		public float GameTimeToSwitchFrames { get; set; }
		
		public AnimationDefinition ()
		{
		}
		
	}
}

