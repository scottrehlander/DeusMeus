using System;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public class LoadingScene : GameScene
	{
		
		private FontMap _fontMap = new FontMap( new Font( FontAlias.System, 24, FontStyle.Bold ), 128 );
		private Type _type;
		private WorldMapDefinition _map;
		private Vector2? _startingPosition = null;
		
		public LoadingScene(Type type, WorldMapDefinition map) : this(type, map, null)
		{
		}
		
		public LoadingScene(Type type, WorldMapDefinition map, Vector2? startingPosition)
		{
			_type = type;
			_map = map;
			if(startingPosition.HasValue)
			{
				_startingPosition = startingPosition.Value;
			}
			
			this.ScheduleUpdate(1);
			this.RegisterDisposeOnExitRecursive();
			
			SetupScene();
		}
		
		public void SetupScene()
		{
			this.Camera.SetViewFromViewport();
			
			var num = (new Random().NextFloat() * 100).ToString();
			if(num.Contains ("."))
			{
				num = num.Substring(0, num.IndexOf('.'));
			}
			Label label = new Label("Loading...\n\nTip #" + num, _fontMap);
			label.HeightScale = 1F;
			label.Color = Colors.White;
			label.Position = new Vector2(Camera2D.Center.X - 50, Camera2D.Center.Y + 20);
			
			base.AddChild(label);
		}
		
		bool _sceneCreated = false;
		public override void Update (float dt)
		{
			if(!_sceneCreated)
			{
				_sceneCreated = true;
				
				new System.Threading.Thread(new System.Threading.ThreadStart(() => {
					// Create an instance of the scene and pass in a map
					Scene scene;
					if(_startingPosition.HasValue)
					{
						scene = Activator.CreateInstance(_type, new object[] { _map, _startingPosition } ) as Scene;
					}
					else
					{
						scene = Activator.CreateInstance(_type, new object[] { _map } ) as Scene;
					}
					Director.Instance.ReplaceScene(scene as Scene);
				})).Start();
			}
		}
		
	}
}

