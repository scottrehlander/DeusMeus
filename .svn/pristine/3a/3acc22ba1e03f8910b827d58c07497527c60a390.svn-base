using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core;
using System.Collections.Generic;

namespace Rp2d
{
	// A class to provide access to a singleton DialogDisplayer
	public static class DialogManager
	{
		// The instance we care about
		private static DialogDisplayer _instance;
		public static DialogDisplayer Instance
		{
			get 
			{
				if(_instance == null)
				{
					_instance = new DialogDisplayer();
				}
				return _instance;
			}
		}
		
		// Some access to the instance
		public static void SetSpeakingNpc(Npc npc)
		{
			Instance.SetSpeakingNpc(npc);
		}
		
		public static bool IsNpcSpeaking()
		{
			if(Instance.SpeakingNpc != null)
			{
				return true;
			}
			return false;
		}
		
	}
	
	public class DialogDisplayer : Node
	{
		// If this object is not null then an NPC is speaking
		private Npc _speakingNpc;
		public Npc SpeakingNpc { get { return _speakingNpc; } }
		
		// Save the sprite and texture that is reuse constantly
		private Texture2D _dialogBackground;
		private TextureInfo _dialogBackgroundTi;
		private SpriteUV _dialogBackgroundSprite;
		private FontMap _dialogFontMap = new FontMap( new Font( FontAlias.System, 32, FontStyle.Bold ), 512 );
		
		// Track the sprites that we need to clean up when the NPC is done talking
		private List<Node> _currentDialogSprites;
		
		// Control when the dialog is opened and closed
		private float _minimumDisplayTime = 1F;
		private float _totalDisplayTime = 0F;
		
		private float _minimumTimeBetweenDialogs = 1F;
		private float _timeBetweenDialogs = 0F;
		
		
		public DialogDisplayer()
		{
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);
			
			// Assume a 0 margin
			_dialogBackground = new Texture2D("/Application/Content/GameMaps/DialogBackground.png", false);
			
			// Figure out what tile this is from our texture collection
			_dialogBackgroundTi = new TextureInfo(_dialogBackground);
		}
		
		public void SetSpeakingNpc(Npc npc)
		{
			// Don't open a dialog if one literally -just- closed
			if(_timeBetweenDialogs < _minimumTimeBetweenDialogs)
			{
				return;
			}
			
			// Don't display anything if they have nothing to say
			var npcDialog = npc.GetDialog();
			if(npcDialog.Equals(string.Empty))
			{
				return;
			}
			
			_speakingNpc = npc;
			_currentDialogSprites = new List<Node>();
			_totalDisplayTime = 0;
			_timeBetweenDialogs = 0;
			
			// Add the dialog background
			if(_dialogBackgroundSprite == null)
			{
				_dialogBackgroundSprite = new SpriteUV(_dialogBackgroundTi);
				_dialogBackgroundSprite.Quad.S = _dialogBackgroundTi.TextureSizef * .8F;
			}
			_dialogBackgroundSprite.Position = new Vector2(Director.Instance.CurrentScene.Camera2D.Center.X - (_dialogBackgroundTi.TextureSizef.X * .8F / 2),
			                               Director.Instance.CurrentScene.Camera2D.Center.Y - (_dialogBackgroundTi.TextureSizef.Y * .8F / 2)
			                              - Director.Instance.GL.Context.GetViewport().Height / 4);
			Director.Instance.CurrentScene.AddChild(_dialogBackgroundSprite); 
			
			// Add the dialog text
			float horizontalMargin = 30;
			float textY = Director.Instance.CurrentScene.Camera2D.Center.Y - (_dialogBackgroundTi.TextureSizef.Y * .8F / 2)
		                              	- Director.Instance.GL.Context.GetViewport().Height / 4 + (_dialogBackgroundTi.TextureSizef.Y * .8F) - 85;
			foreach(string textLine in npcDialog.Split(new string[] { @"\r\n" }, StringSplitOptions.None))
			{
				Label label = new Label(textLine.Trim(),
		       		_dialogFontMap
            	);
				label.HeightScale = .6F;
				label.Color = Colors.Grey10;
				label.Position = new Vector2(Director.Instance.CurrentScene.Camera2D.Center.X - (_dialogBackgroundTi.TextureSizef.X * .8F / 2) + 65,
	                               		textY);
				
				textY -= horizontalMargin;
				Director.Instance.CurrentScene.AddChild(label);				
				_currentDialogSprites.Add(label);
			}
		}
		
		public void StopSpeaking()
		{
			foreach(Node node in _currentDialogSprites)
			{
				Director.Instance.CurrentScene.RemoveChild(node, true);
			}
			_currentDialogSprites.Clear();
			
			Director.Instance.CurrentScene.RemoveChild(_dialogBackgroundSprite, false);
			
			_speakingNpc = null;
		}
		
		public override void Update(float dt)
		{
			if(_speakingNpc == null)
			{
				_timeBetweenDialogs += dt;
				return;
			}
			
			_totalDisplayTime += dt;
			
			// Don't let the window exit immediately
			if(_totalDisplayTime < _minimumDisplayTime)
			{
				return;
			}
			
			
			// Check for an action button, if it is pressed then let's remove the dialog sprite
			if(GameInput.GetInputValue(GameInputControls.Action) != 0)
			{
				StopSpeaking();
			}
			
			base.Update(dt);
		}
		
	}
}

