using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Input;
using System.Collections.Generic;

namespace Rp2d
{
	public class TitleScene : MenuScene
	{
		
		Texture2D texture;
  		TextureInfo ti;
		
		public TitleScene ()
		{
			this.Camera.SetViewFromViewport();
			texture = new Texture2D("/Application/Content/ScratchSpace/pic1.png",false);
			ti = new TextureInfo(texture);
			SpriteUV sprite = new SpriteUV(ti);
			sprite.Quad.S = ti.TextureSizef;
			sprite.Position = new Vector2(110,150);
			this.AddChild(sprite);
			this.ScheduleUpdate(1);
			this.RegisterDisposeOnExitRecursive();
			
			// DEBUG
			FPSDisplayer fps = new FPSDisplayer();
  			fps.Position = new Vector2(0,0);
			AddChild(fps);
		}
		
		~TitleScene ()
		{
			ti.Dispose();
			texture.Dispose();
		}
		
		public override void Update (float dt)
		{
			if((GamePad.GetData(0).Buttons & GamePadButtons.Circle) != 0)
			{
				// Load the global assets
				EnemyManager.LoadEnemyRepository();
				var map = WorldMapManager.GetMapFromId(1);
				Director.Instance.ReplaceScene(new LoadingScene(typeof(GameplayActionScene), map));
			}
			if((GamePad.GetData(0).Buttons & GamePadButtons.Triangle) != 0)
			{
				Director.Instance.ReplaceScene(new TestScene());
			}
			
			// Exit on Left Bumper just for Pappas
			if((GamePad.GetData(0).Buttons & GamePadButtons.L) != 0)
			{
				Environment.Exit(0);
			}
		}
	}
}

