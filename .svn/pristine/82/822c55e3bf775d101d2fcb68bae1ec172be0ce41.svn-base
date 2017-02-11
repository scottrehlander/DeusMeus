using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using System.Collections.Generic;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public class AnimatedEntity : EntityBase, IPoolable
	{
		
		private TextureInfo _textureInfo;
		private Texture2D _texture;
		private Dictionary<string, List<Vector2i>> _sprites;
		private Dictionary<string, float> _timeForFrameSwitch;
		protected Vector2 _previousPosition;
		//private Vector2 _scaleVector = new Vector2(32, 32);
		
		// Track the time elapsed for this frame
		private float _animationTimeElapsed;
		public float AnimationTimeElapsed 
		{
			get { return _animationTimeElapsed; }
		}
		
		public bool PauseAnimation { get; set; }
		
		// The sprite for the entity
		public SpriteTile EntitySprite { get; set; }
		
		// Define a property that will autoset the current frame number back to 0
		//  if we try to set it higher than the frames for this animation
		private int _currentFrameNumber = -1;
		protected int CurrentFrameNumber
		{
			get { return _currentFrameNumber; }
			set 
			{
				var oldValue = _currentFrameNumber;
				if(value > _sprites[CurrentAnimation].Count - 1)
				{
					_animationTimeElapsed = 0;
					_currentFrameNumber = 0;
				}
				else
				{
					_currentFrameNumber = value;
				}
				
				if(oldValue != _currentFrameNumber)
				{
					GetEntity();
				}
			}
		}
		
		protected string _textureFilename;
		protected string _name;
		protected Vector2i _spriteSheetDimensions;
		protected Vector2 _tilePixelDimensions;
		
		private string _currentAnimation = "";
		public string CurrentAnimation 
		{ 
			get{ return _currentAnimation; }
			set 
			{
				// Make sure this animation exists
				if(!_sprites.ContainsKey(value))
				{
					throw new Exception("Animation " + value + " was not initialized");
				}
			
				_currentAnimation = value; 
				CurrentFrameNumber = 0;
			}
		}
		
		public AnimatedEntity()
		{
			PauseAnimation = false;
		}
		
		~AnimatedEntity()
		{
			_texture.Dispose();
			_textureInfo.Dispose ();
		}
		
		public void InitializeEntity(string textureFilename, Vector2i spriteSheetDimensions,
		                             Vector2 tilePixelDimensions)
		{
			_textureFilename = textureFilename;
			_spriteSheetDimensions = spriteSheetDimensions;
			_tilePixelDimensions = tilePixelDimensions;
			
			// Initialize the frame count maximums	
			_timeForFrameSwitch = new Dictionary<string, float>();
			
			// Initialize the sprites collection
			_sprites = new Dictionary<string, List<Vector2i>>();
			
			var textureInfo = TextureManager.CreateTexture2D(_textureFilename, false, _spriteSheetDimensions, false);
			_texture = textureInfo.Texture;
			_textureInfo = textureInfo.Info;
		}
		
		protected void SetAnimation(string animationName, int row, int startColumn, int endColumn, float timeForFrameSwitch)
		{
			_timeForFrameSwitch.Add(animationName, timeForFrameSwitch);
			
			List<Vector2i> frameList = new List<Vector2i>();
			
			for(int i = startColumn; i <= endColumn; i++)
			{
				frameList.Add(new Vector2i(i, row));
			}
			
			_sprites.Add(animationName, frameList);
		}
		
		public SpriteTile GetEntity(int x, int y)
		{
			var spriteTile = new SpriteTile(_textureInfo);
			spriteTile.TileIndex2D = new Vector2i(x - 1, y - 1);
			spriteTile.Quad.S = _tilePixelDimensions; //_scaleVector;
			return spriteTile;
		}
		
		public SpriteTile GetEntity()
		{
			if(_sprites == null || !_sprites.ContainsKey(CurrentAnimation) || 
			   _sprites[CurrentAnimation] == null ||
			   _sprites[CurrentAnimation].Count == 0)
			{
				throw new Exception("Animation " + CurrentAnimation + " was not initialized.");
			}
			
			return GetEntity(_sprites[CurrentAnimation][CurrentFrameNumber].X, 
			            _sprites[CurrentAnimation][CurrentFrameNumber].Y);
		}
		
		public void IncrementTimeElapsed(float timeElapsed)
		{
			// Don't increment the time elapsed if the animation is pause
			if(PauseAnimation)
			{
				return;
			}
			
			_animationTimeElapsed += timeElapsed;
			
			// Figure out which frame we should be on based on the amount of time 
			//  that has elapsed for this animation
			var timeForFrameSwitch = _timeForFrameSwitch[CurrentAnimation];
			var frameToBeOn = (_animationTimeElapsed / timeForFrameSwitch);
			
			var floorResult = System.Math.Floor(frameToBeOn);
			CurrentFrameNumber = Convert.ToInt32(System.Math.Floor(frameToBeOn));
		}
		
		// Set the sprite to the current animation an frame
		public override void Update(float dt)
		{
			IncrementTimeElapsed(dt);
			EntitySprite.TileIndex2D = GetEntity().TileIndex2D;
		}
		
		protected AnimationDefinition[] CreateBasicAnimationDefinitions()
		{
			// Define the basic animations for a RoamingNpc
			return new AnimationDefinition[]
			{
				new AnimationDefinition()
				{
					AnimationName = "walk_down",
					ImageRow = 4,
					ImageStartColumn = 1,
					ImageEndColumn = 3,
					GameTimeToSwitchFrames = .25F
				},
				new AnimationDefinition()
				{
					AnimationName = "walk_left",
					ImageRow = 3,
					ImageStartColumn = 1,
					ImageEndColumn = 3,
					GameTimeToSwitchFrames = .25F
				},
				new AnimationDefinition()
				{
					AnimationName = "walk_right",
					ImageRow = 2,
					ImageStartColumn = 1,
					ImageEndColumn = 3,
					GameTimeToSwitchFrames = .25F
				},
				new AnimationDefinition()
				{
					AnimationName = "walk_up",
					ImageRow = 1,
					ImageStartColumn = 1,
					ImageEndColumn = 3,
					GameTimeToSwitchFrames = .25F
				},
				new AnimationDefinition()
				{
					AnimationName = "idle",
					ImageRow = 4,
					ImageStartColumn = 2,
					ImageEndColumn = 2,
					GameTimeToSwitchFrames = .25F
				}
			};
		}
	
		public virtual void Reset()
		{
			return;
		}
		
		public bool IsActive { get; set; }
		public string Key { get { return _textureFilename; } }
		
	}
}