using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using System.Xml;
using Sce.PlayStation.Core;

namespace Rp2d
{
	public static class WorldMapManager
	{
		
		private const string _worldMapFilename = "/Application/Content/GameMaps/WorldMap.xml";
		
		
		// Keep a list of all of the map definitions.  We may need to lazy load this in the future
		private static List<WorldMapDefinition> _worldMap;
		public static List<WorldMapDefinition> WorldMap
		{ 
			get
			{
				if(_worldMap == null)
				{
					_worldMap = LoadWorldMap();
				}
				return _worldMap;
			}
		}

		// Load the world map from the XML
		private static List<WorldMapDefinition> LoadWorldMap()
		{
			List<WorldMapDefinition> worldMapList = new List<WorldMapDefinition>();
			
			var worldMapNodeList = SerializationHelpers.GetXmlNodes<WorldMapDefinition>(_worldMapFilename);
			foreach(XmlNode worldMapNode in worldMapNodeList)
			{
				worldMapList.Add(SerializationHelpers.ConvertNode<WorldMapDefinition>(worldMapNode));
			}
			
			return worldMapList;
		}
		
		// Get the map given an id
		public static WorldMapDefinition GetMapFromId(int id)
		{
			foreach(var map in WorldMap)
			{
				if(map.MapId == id)
				{
					return map;
				}
			}
			
			// We didn't find the map
			return null;
		}
		
	}
	
	public class WorldMapDefinition
	{
		public int MapId { get; set; }
		public string MapName { get; set; }
		public string MapFilename { get; set; }
		public string MapBackgroundSongFilename { get; set; }
		public WorldMapExitArea[] WorldMapExitAreas { get; set; }
		
		public WorldMapExitArea GetExitAreaFromPosition(ICollidable collidable)
		{
			foreach(var exitArea in WorldMapExitAreas)
			{
				if(CollisionManager.RectanglesIntersect(collidable.CollisionRectangle, exitArea.ExitAreaRectangleInPixles))
				{
					return exitArea;
				}
			}
			
			return null;
		}
	}
	
	public class WorldMapExitArea
	{
		private Rectangle? _exitAreaRectangleInPixels;
		public Rectangle ExitAreaRectangleInPixles 
		{
			get
			{
				// Build the rectangle and return it
				if(!_exitAreaRectangleInPixels.HasValue)
				{
					_exitAreaRectangleInPixels = new Rectangle(MapExitAreaX, MapExitAreaY,
					                                           MapExitAreaWidth, MapExitAreaHeight);
				}
				return _exitAreaRectangleInPixels.Value;
			}
		}
					
		public int MapExitAreaX { get; set; }
		public int MapExitAreaY { get; set; }
		public int MapExitAreaWidth { get; set; }
		public int MapExitAreaHeight { get; set; }
		public int MapToExitTo { get; set; }
		public float MapToExitToStartingXInPixels { get; set; }
		public float MapToExitToStartingYInPixels { get; set; }
	}
}