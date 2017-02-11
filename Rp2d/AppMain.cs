//using System;
//using System.Collections.Generic;
//
//using Sce.PlayStation.Core;
//using Sce.PlayStation.Core.Environment;
//using Sce.PlayStation.Core.Graphics;
//using Sce.PlayStation.Core.Input;
//
//namespace Rp2d
//{
//	public class AppMain
//	{
//		private static GraphicsContext _graphics;
//		private static Game _game;
//		
//		public static void Main (string[] args)
//		{
//			Initialize ();
//
//			while (true) {
//				SystemEvents.CheckEvents ();
//				LoadContent();
//				Update ();
//				Render ();
//			}
//		}
//
//		public static void Initialize ()
//		{
//			// Set up the graphics system
//			_graphics = new GraphicsContext ();
//			_game = new Game(_graphics);
//		}
//		
//		public static void LoadContent()
//		{
//			// Load content
//			_game.LoadContent();
//		}
//		
//		public static void Update ()
//		{
//			_game.Update();
//		}
//
//		public static void Render ()
//		{
//			_game.Draw();
//			
//		}
//	}
//}
