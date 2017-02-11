using System;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public class Npc : AnimatedEntity, ICollidable
	{	
		
		// Track the dialog that this npc will say if approached
		protected int _currentDialogId = -1;
		public int CurrentDialogId 
		{ 
			get { return _currentDialogId; }
			set { _currentDialogId = value; }
		}
		
		// Store the possible dialog that this npc can speak
		public NpcDialog[] DialogList { get; set; }
		
		// Define a new rectangle to check collisions with the hero
		protected Rectangle _newCollisionRectangleToCheck;
		
		public bool IsStationary { get { return false; } }
		
		// Define the collision rectangle for this sprite
		protected Rectangle _collisionRectangle;
		public Rectangle CollisionRectangle 
		{ 
			get 
			{
				_collisionRectangle.X = EntitySprite.Position.X;
				_collisionRectangle.Y = EntitySprite.Position.Y;
				return _collisionRectangle; 
			} 
		}
		
		
		public Npc ()
		{
		}
		
		
		public string GetDialog()
		{
			if(DialogList == null)
			{
				return "";
			}
			
			foreach(var dialog in DialogList)
			{
				if(dialog.Id == _currentDialogId)
				{
					return dialog.Text;
				}
			}
			
			// If it's an invalid dialog id, don't crash just don't say anything
			return "";
		}
		
	}
}

