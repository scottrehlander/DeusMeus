using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core;

namespace Rp2d
{
public class FPSDisplayer : SpriteUV
 {
  TextureInfo _ti;
  
  public FPSDisplayer ()
  {
   Texture2D texture = new Texture2D(150,1000,false,
                                     PixelFormat.Rgba);
   _ti = new TextureInfo(texture);
   
   this.TextureInfo = _ti;
   this.Quad.S = new Vector2(150,100);
   Scheduler.Instance.ScheduleUpdateForTarget(this,1,false);
  }
  
  public override void Update (float dt)
  {
   _ti.Dispose();
   Image img = new Image(ImageMode.Rgba, new ImageSize(150,100),
                         new ImageColor(255,255,255,0));
   img.DrawText("FPS:" + (1/dt).ToString(), 
                new ImageColor(255,255,255,255),
                new Font(FontAlias.System,32,FontStyle.Bold),
                new ImagePosition(0,0));
   //Console.Out.WriteLine((double)GC.GetTotalMemory(true)/(1024.0*1024.0));
   
   Texture2D texture = new Texture2D(150,100,false,
                                     PixelFormat.Rgba);
      texture.SetPixels(0,img.ToBuffer(),PixelFormat.Rgba);
   img.Dispose();
   _ti = new TextureInfo(texture);
   this.TextureInfo  = _ti;

   base.Update (dt);
  }
 }
}


