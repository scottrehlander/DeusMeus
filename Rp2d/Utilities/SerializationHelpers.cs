using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace Rp2d
{
	public static class SerializationHelpers
	{
		public static XmlNodeList GetXmlNodes<T>(string filename)
		{
			if(!File.Exists(filename))
			{
				return null;
			}
			
			XmlDocument doc;
			
			using (var stream = System.IO.File.OpenRead(filename))
			{
		    	doc = new XmlDocument();
				doc.Load(stream);
			}
			
			return doc.SelectNodes("//" + typeof(T).ToString().Split('.')[1]);
		}
		
		public static T ConvertNode<T>(XmlNode node) where T: class
		{
			T result;
			using(MemoryStream stm = new MemoryStream())
			{
			    using(StreamWriter stw = new StreamWriter(stm))
				{
				    stw.Write(node.OuterXml);
				    stw.Flush();
				
				    stm.Position = 0;
				
					// Define the root
					XmlRootAttribute xRoot = new XmlRootAttribute();
					xRoot.ElementName = typeof(T).ToString().Split('.')[1];
					xRoot.IsNullable = true;
					
				    XmlSerializer ser = new XmlSerializer(typeof(T), xRoot);
			    	result = (ser.Deserialize(stm) as T);
				}
			}
		    return result;
		}
	}
}

