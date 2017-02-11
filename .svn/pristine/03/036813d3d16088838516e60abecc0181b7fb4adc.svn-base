using System;
using System.Collections.Generic;

namespace Rp2d
{
	public class Enemy : CollidableAnimatedEntity
	{
		public int SpawnPointId { get; set; }
		public int Id { get; private set; }
		
		public Enemy ()
		{
			Id = GetNextId();
		}
		
		private static int IdCounter = 0;
		public static int GetNextId()
		{
			if(IdCounter > 5000)
			{
				IdCounter = 0;
			}
			
			return IdCounter++;
		}
		
	}
}

