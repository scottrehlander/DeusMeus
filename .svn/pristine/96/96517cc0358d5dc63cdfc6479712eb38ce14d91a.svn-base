using System;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Input;

namespace Rp2d
{
	public class TestScene : GameScene
	{
		Texture2D texture;
  		TextureInfo ti;
		
		SpriteUV _sprite;
		  
		public TestScene ()
		{
			this.Camera.SetViewFromViewport();
			texture = new Texture2D("/Application/Content/ScratchSpace/Map2.png",false);
			ti = new TextureInfo(texture);
			_sprite = new SpriteUV(ti);
			_sprite.Quad.S = ti.TextureSizef;
			_sprite.Position = new Vector2(0,0);
			this.AddChild(_sprite);
			this.ScheduleUpdate(1);
			this.RegisterDisposeOnExitRecursive();
			
			// DEBUG
			FPSDisplayer fps = new FPSDisplayer();
  			fps.Position = new Vector2(0,0);
			AddChild(fps);
		}
		
		~TestScene ()
		{
			ti.Dispose();
			texture.Dispose();
		}
		
		public override void Update (float dt)
		{
			int xMovement = 0;
			int yMovement = 0;
			if((GamePad.GetData(0).Buttons & GamePadButtons.Right) != 0)
			{	
				xMovement = -5;
			}
			if((GamePad.GetData(0).Buttons & GamePadButtons.Left) != 0)
			{
				xMovement = 5;
			}
			if((GamePad.GetData(0).Buttons & GamePadButtons.Up) != 0)
			{
				yMovement = -5;
			}
			if((GamePad.GetData(0).Buttons & GamePadButtons.Down) != 0)
			{
				yMovement = 5;
			}
			
			_sprite.Position = new Vector2(_sprite.Position.X + xMovement, _sprite.Position.Y + yMovement);
		}
		

	}
}

