using System;
using System.Collections.Generic;

namespace Rp2d
{
	public static class NpcManager
	{
		private static List<Npc> _npcs;
		public static List<Npc> Npcs { get { return _npcs; } }
		
		public static void LoadNpcs(string npcFilename)
		{
			// Load the NPCs
			var npcList = SerializationHelpers.GetXmlNodes<RoamingNpcInfo>(npcFilename);
			if(npcList != null)
			{
				foreach(System.Xml.XmlNode npcNode in npcList)
				{
					AddNpc(new RoamingNpc(SerializationHelpers.ConvertNode<RoamingNpcInfo>(npcNode)));
				}
			}
		}
		
		private static void AddNpc(Npc npc)
		{
			if(_npcs == null)
			{
				_npcs = new List<Npc>();
			}
			
			_npcs.Add(npc);
		}
		
		public static void RemoveAllNpcs()
		{
			if(_npcs == null)
			{
				_npcs = new List<Npc>();
			}
			
			// Cleanup the npc nodes and clear the list
			foreach(var npc in _npcs)
			{
				npc.Cleanup();
			}
			_npcs.Clear();
		}
	}
}

