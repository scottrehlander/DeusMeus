using System;
using System.Collections.Generic;

namespace Rp2d
{
	public static class GameInput
	{
		
		private static IControllerQueryBase _controllerQueryAgent;
		
		private static Dictionary<GameInputControls, int> _inputValues;
		
		public static void Initialize()
		{
			_controllerQueryAgent = new PsController();
		}
		
		public static void UpdateGameInput()
		{
			// Set the game control values with an input class
			_inputValues = _controllerQueryAgent.GetInputs();
		}
		
		public static int GetInputValue(GameInputControls control)
		{
			return _inputValues.ContainsKey(control) ? 
				_inputValues[control] : 0;
		}
		
		public static void SetControllerType(IControllerQueryBase controller)
		{
			_controllerQueryAgent = controller;	
		}
		
	}
}

