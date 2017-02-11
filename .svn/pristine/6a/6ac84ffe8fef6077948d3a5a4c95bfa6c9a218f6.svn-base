using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Input;

namespace Rp2d
{
	public class PsController : IControllerQueryBase
	{
		private Dictionary<GameInputControls, int> _gameInputControls = new Dictionary<GameInputControls, int>();
		private float _analogThreshold = .8F;
		
		public PsController ()
		{
		}
		
		public Dictionary<GameInputControls, int> GetInputs()
		{
			_gameInputControls.Clear();
			
			// Query gamepad for current state
			var gamePadData = GamePad.GetData(0);

			#region Track Pad
			
			if((gamePadData.Buttons & GamePadButtons.Left) != 0 ||
			   gamePadData.AnalogLeftX < -_analogThreshold)
			{
				_gameInputControls.Add(GameInputControls.MoveLeft, 1);
			}
			if((gamePadData.Buttons & GamePadButtons.Right) != 0 ||
			   gamePadData.AnalogLeftX > _analogThreshold)
			{
				_gameInputControls.Add(GameInputControls.MoveRight, 1);
			}
			if((gamePadData.Buttons & GamePadButtons.Up) != 0 || 
			   gamePadData.AnalogLeftY < -_analogThreshold)
			{
				_gameInputControls.Add(GameInputControls.MoveUp, 1);
			}
			if((gamePadData.Buttons & GamePadButtons.Down) != 0 ||
			   gamePadData.AnalogLeftY > _analogThreshold)
			{
				_gameInputControls.Add(GameInputControls.MoveDown, 1);
			}
			
			#endregion
			
			
			#region Action Buttons
			
			if((gamePadData.Buttons & GamePadButtons.Cross) != 0)
			{
				_gameInputControls.Add(GameInputControls.Action, 1);
			}
			if((gamePadData.Buttons & GamePadButtons.L) != 0)
			{
				_gameInputControls.Add(GameInputControls.Exit, 1);
			}
			
			#endregion
			
			return _gameInputControls;
		}
	}
}

