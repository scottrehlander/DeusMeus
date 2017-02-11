using System;
using System.Collections.Generic;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Rp2d
{
	public static class TextureManager
	{
		private static Dictionary<string, Texture2DAndTextureInfo> _textures;
		private static List<Texture2DAndTextureInfo> _unkeyedTextures;
		
		public static Texture2DAndTextureInfo CreateTexture2D(string filename, bool mipmap, bool reuseTexture)
		{
			return CreateTexture2D(filename, mipmap, null, reuseTexture);
		}
		
		public static Texture2DAndTextureInfo CreateTexture2D(string filename, bool mipmap, Vector2i? numTiles, bool reuseTexture)
		{
			if(_textures == null)
			{
				_textures = new Dictionary<string, Texture2DAndTextureInfo>();
			}
			
			if(reuseTexture)
			{
				if(_textures.ContainsKey(filename))
				{
					return _textures[filename];
				}
			}
			
			Texture2D texture = new Texture2D(filename, mipmap);
			
			TextureInfo ti;
			if(numTiles.HasValue)
			{
				ti = new TextureInfo(texture, numTiles.Value);
			}
			else
			{
				ti = new TextureInfo(texture);
			}
			
			var textureAndInfo = new Texture2DAndTextureInfo() 
	            {
					Texture = texture,
					Info = ti
				};
			if(!_textures.ContainsKey(filename))
			{
				_textures.Add(filename, textureAndInfo);
			}
			
			return textureAndInfo;
		}
		
		public static Texture2DAndTextureInfo CreateTexture2D(uint rgba, int width, int height)
		{
			if(_unkeyedTextures == null)
			{
				_unkeyedTextures = new List<Texture2DAndTextureInfo>();
			}
			
			Texture2DAndTextureInfo info = new Texture2DAndTextureInfo();
			
			uint[] vBuffer = new uint[width * height];
			
			//Fill buffer to red
			for(int i = 0; i < width * height; i++) 
			{
				vBuffer[i] = rgba;
			}
			
			//Creature a RGBA texture.
			info.Texture = new Texture2D(width, height, false, PixelFormat.Rgba);
			info.Texture.SetPixels(0, vBuffer);
			info.Info = new TextureInfo(info.Texture);
			
			_unkeyedTextures.Add(info);
			
			return info;
		}
	
		public static void DisposeTextures()
		{
			if(_textures != null)
			{
				foreach(var textureInfo in _textures)
				{
					textureInfo.Value.Info.Dispose();
					textureInfo.Value.Texture.Dispose();
				}
				
				_textures.Clear();
			}
			if(_unkeyedTextures != null)
			{
				foreach(var textureInfo in _unkeyedTextures)
				{
					textureInfo.Info.Dispose();
					textureInfo.Texture.Dispose();
				}
				
				_unkeyedTextures.Clear();
			}
		}
		
	}
	
	public class Texture2DAndTextureInfo
	{
		public Texture2D Texture { get; set; }
		public TextureInfo Info { get; set; }
	}
	
}

