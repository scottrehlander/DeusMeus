using System;
using System.Collections.Generic;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.GameEngine2D;
using System.Threading;

namespace Rp2d
{
	public static class Support
    {
    	public static TextureFilterMode DefaultTextureFilterMode = TextureFilterMode.Linear;
    	public static Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();
    	public static Dictionary<string, TextureInfo> TextureInfoCache = new Dictionary<string, TextureInfo>();

        public static void PrecacheTiledSprite(string filename, int x, int y)
        {
        	if (TextureCache.ContainsKey(filename) == false)
			{
	            TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(x, y));
			}
		}

        public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile SpriteFromFile(string filename)
        {
        	if (TextureCache.ContainsKey(filename) == false)
			{
	            TextureCache[filename] = new Texture2D(filename, false);
	            TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(1, 1));
			}
			
            var tex = TextureCache[filename];
            var info = TextureInfoCache[filename];

   //       Vector2i tilesize=new Vector2i(256,256);
   //       if ( info.TextureSize )
   //       {
   //       }

            var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info, };
            
			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);
//          result.Quad.S = info.TextureSizef;
			
			// DEBUG: testing for current assets
			result.Scale = new Vector2(1.0f);
			
			tex.SetFilter(DefaultTextureFilterMode);
			
            return result;
        }
      
        public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile UnicolorSprite(string name, byte r, byte g, byte b, byte a)
        {
        	uint color = (uint)(a << 24 | b << 16 | g << 8 | r);
				
        	if (TextureCache.ContainsKey(name) == false)
			{
	            TextureCache[name] = GraphicsContextAlpha.CreateTextureUnicolor(color);
	            TextureInfoCache[name] = new TextureInfo(TextureCache[name], new Vector2i(1, 1));
			}
			
            var tex = TextureCache[name];
            var info = TextureInfoCache[name];
            var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info };
            
			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);
			
			tex.SetFilter(DefaultTextureFilterMode);
			
            return result;
        }
    
        public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteUV SpriteUVFromFile(string filename)
        {
        	if (TextureCache.ContainsKey(filename) == false)
			{
	            TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename]);
			}
			
            var tex = TextureCache[filename];
            var info = TextureInfoCache[filename];
            var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteUV() { TextureInfo = info };
            
			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);
			
			tex.SetFilter(DefaultTextureFilterMode);
			
            return result;
        }
        
        public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile TiledSpriteFromFile(string filename, int x, int y)
        {
        	if (TextureCache.ContainsKey(filename) == false)
			{
	            TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename], new Vector2i(x, y));
			}
			
            var tex = TextureCache[filename];
            var info = TextureInfoCache[filename];
            var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile() { TextureInfo = info };

			result.TileIndex2D = new Vector2i(0, 0);

			result.Quad.S = new Vector2(info.Texture.Width / x, info.Texture.Height / y);
			
			tex.SetFilter(DefaultTextureFilterMode);
			
            return result;
        }

		public static int IncrementTile(Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile sprite, int steps, int min, int max, bool looping)
		{
			int x = sprite.TextureInfo.NumTiles.X;
			int y = sprite.TextureInfo.NumTiles.Y;
			
			int current = sprite.TileIndex2D.X + sprite.TileIndex2D.Y * x;
			
			if (looping)
			{
				current -= min;
				current += steps;
				current %= max - min;
				current += min;
			}
			else
			{
				current += steps;
				current = System.Math.Min(current, max - 1);
			}
			
            sprite.TileIndex1D = current;
			
			return current;
		}
		
		public static void SetTile(Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile sprite, int n)
		{
            sprite.TileIndex1D = n;
		}
		
		public static int GetTileIndex(Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile sprite)
		{
            return sprite.TileIndex1D;
		}
		
		public class AnimationAction
			: Sce.PlayStation.HighLevel.GameEngine2D.ActionBase
		{
			int animation_start;
			int animation_end;
			Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile attached_sprite;
			float counter;
			float frame_time;
			float speed;
			bool looping;
			
			public AnimationAction(Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile sprite, float seconds)
				: this(sprite, 0, sprite.TextureInfo.NumTiles.X * sprite.TextureInfo.NumTiles.Y, seconds)
			{
			}
			
			public AnimationAction(Sce.PlayStation.HighLevel.GameEngine2D.SpriteTile sprite, int a, int b, float seconds, bool looping = false)
			{
				this.looping = looping;
				speed = 1.0f;
				
				attached_sprite = sprite;
				
				int min = System.Math.Min(a, b);
				int max = System.Math.Max(a, b);
				int frames = System.Math.Max(1, max - min);	
				
				frame_time = seconds / (float)frames;
				animation_start = min;
				animation_end = max;
				
				Reset();
			}
			
			public override void Run()
			{
				base.Run();
				Reset();
			}
			
			public override void Update(float dt)
			{
				dt *= speed;
				
				base.Update(dt);
				
				counter += dt;
				
				int frames = 0;
				while (frame_time > 0.0f && counter > frame_time)
				{
					counter -= frame_time;
					frames += 1;
				}
				
				int tile_index = IncrementTile(attached_sprite, frames, animation_start, animation_end, looping);
				
				if (!looping && tile_index == animation_end - 1)
				{
					Stop();
				}
			}
			
			public void SetSpeed(float speed)
			{
				this.speed = speed;
			}
			
			public void Reset()
			{
				counter = 0.0f;
				SetTile(attached_sprite, animation_start);
			}
		}

		public class SoundSystem
		{
			public static SoundSystem Instance = new SoundSystem("/Application/assets/sounds/");

			public string AssetsPrefix;
			public Dictionary<string, SoundPlayer> SoundDatabase;

			public SoundSystem(string assets_prefix)
			{
				AssetsPrefix = assets_prefix;
				SoundDatabase = new Dictionary<string, SoundPlayer>();
			}

			public void CheckCache(string name)
			{
				if (SoundDatabase.ContainsKey(name))
					return;

				var sound = new Sound(AssetsPrefix + name);
				var player = sound.CreatePlayer();
				SoundDatabase[name] = player;
			}

			public void Play(string name)
			{
				CheckCache(name);

				// replace any playing instance
				SoundDatabase[name].Stop();
				SoundDatabase[name].Play();
				SoundDatabase[name].Volume = 0.5f;
			}
		
			public void Stop(string name)
			{
				CheckCache(name);
				SoundDatabase[name].Stop();
			}
		
			public void PlayNoClobber(string name)
			{
				CheckCache(name);
				if (SoundDatabase[name].Status == SoundStatus.Playing)
					return;
				SoundDatabase[name].Play();
			}
		}
		
		public class MusicSystem
		{
			public static MusicSystem Instance = new MusicSystem("/Application/assets/sounds/");

			public string AssetsPrefix;
			public Dictionary<string, BgmPlayer> MusicDatabase;

			public MusicSystem(string assets_prefix)
			{
				AssetsPrefix = assets_prefix;
				MusicDatabase = new Dictionary<string, BgmPlayer>();
			}

			public void StopAll()
			{
				foreach (KeyValuePair<string, BgmPlayer> kv in MusicDatabase)
				{
					kv.Value.Stop();
					kv.Value.Dispose();
				}

				MusicDatabase.Clear();
			}

			public void Play(string name)
			{
				StopAll();

				var music = new Bgm(AssetsPrefix + name);
				var player = music.CreatePlayer();
				MusicDatabase[name] = player;

				MusicDatabase[name].Play();
				MusicDatabase[name].Volume = 0.5f;
			}
		
			public void Stop(string name)
			{
				StopAll();
			}
		
			public void PlayNoClobber(string name)
			{
				if (MusicDatabase.ContainsKey(name))
				{
					if (MusicDatabase[name].Status == BgmStatus.Playing)
						return;
				}

				Play(name);
			}
		}
		
		public class AdjustableDelayAction
			: Sce.PlayStation.HighLevel.GameEngine2D.DelayTime
		{
			public float Speed { get; set; }	
			
			public AdjustableDelayAction()
			{
				Speed = 1.0f;
			}
			
			public override void Update(float dt)
			{
				base.Update(dt * Speed);
			}
		}
		
		public class TextureTileMapManager
		{
			public struct VertexData
			{
				public Vector2 position;
				public Vector2 uv;
			};
			
			public VertexBuffer VertexBuffer;
			public ShaderProgram ShaderProgram;
			public VertexData[] VertexDataArray;
			public ushort[] IndexDataArray;
			
			public class Entry
			{
				public int TilesX;
				public int TilesY;
				public int TileWidth;
				public int TileHeight;
				public List<List<byte>> Data;
			};
			
			public Dictionary<string, Entry> TileData;
			
			public static int ScaleDivisor = 4;
			
			public TextureTileMapManager()
			{
				VertexBuffer = new VertexBuffer(4, 4, VertexFormat.Float2, VertexFormat.Float2);
				ShaderProgram = new ShaderProgram("/Application/assets/offscreen.cgx");
				VertexDataArray = new VertexData[4];
				IndexDataArray = new ushort[4] { 0, 1, 2, 3 };
				
				TileData = new Dictionary<string, Entry>();
			}
			
			public void TestOffscreen(string name, Texture2D texture)
			{
				const int Width = 32;
				const int Height = 32;

				ImageRect old_scissor = Director.Instance.GL.Context.GetScissor();
				ImageRect old_viewport = Director.Instance.GL.Context.GetViewport();
				FrameBuffer old_frame_buffer = Director.Instance.GL.Context.GetFrameBuffer();

				ColorBuffer color_buffer = new ColorBuffer(Width, Height, PixelFormat.Rgba);
				FrameBuffer frame_buffer = new FrameBuffer();
				frame_buffer.SetColorTarget(color_buffer);
				
				Console.WriteLine("SetFrameBuffer(): enter");
				Director.Instance.GL.Context.SetFrameBuffer(frame_buffer);
				Console.WriteLine("SetFrameBuffer(): exit");

				ShaderProgram.SetAttributeBinding(0, "iPosition");
				ShaderProgram.SetAttributeBinding(1, "iUV");
				
				texture.SetWrap(TextureWrapMode.ClampToEdge);
				texture.SetFilter(TextureFilterMode.Linear);
				
				Director.Instance.GL.Context.SetTexture(0, texture);
				Director.Instance.GL.Context.SetVertexBuffer(0, VertexBuffer);
				Director.Instance.GL.Context.SetShaderProgram(ShaderProgram);
				Director.Instance.GL.Context.SetScissor(0, 0, Width, Height);
				Director.Instance.GL.Context.SetViewport(0, 0, Width, Height);

				float uv_x0 = 0.0f;
				float uv_x1 = 1.0f;
				float uv_y0 = 0.0f;
				float uv_y1 = 1.0f;
				
				VertexDataArray[0] = new VertexData() { position = new Vector2(-1.0f, -1.0f), uv = new Vector2(uv_x0, uv_y1) };
				VertexDataArray[1] = new VertexData() { position = new Vector2(-1.0f, +1.0f), uv = new Vector2(uv_x0, uv_y0) };
				VertexDataArray[2] = new VertexData() { position = new Vector2(+1.0f, +1.0f), uv = new Vector2(uv_x1, uv_y0) };
				VertexDataArray[3] = new VertexData() { position = new Vector2(+1.0f, -1.0f), uv = new Vector2(uv_x1, uv_y1) };

				VertexBuffer.SetIndices(IndexDataArray, 0, 0, 4);
				VertexBuffer.SetVertices(VertexDataArray, 0, 0, 4);

				Director.Instance.GL.Context.SetVertexBuffer(0, VertexBuffer);

				Director.Instance.GL.Context.DrawArrays(DrawMode.TriangleFan, 0, 4);

				int count = Width * Height * 4;
				byte[] data = new byte[count];

				Console.WriteLine("ReadPixels(): enter");
				Director.Instance.GL.Context.ReadPixels(data, PixelFormat.Rgba, 0, 0, Width, Height);
				Console.WriteLine("ReadPixels(): exit");

				int nonzero = 0;
				int nonclear = 0;
				for (int i = 0; i < count; ++i)
				{
					if (data[i] != 0)
						nonzero++;
					if (data[i] != 0xfe)
						nonclear++;

					Console.Write("{0} ", data[i]);
					if (i % Width == 0)
						Console.WriteLine("");
				}

				Console.WriteLine("");
				Console.WriteLine("nonzero: {0}, nonclear: {1}", nonzero, nonclear);
						
				Director.Instance.GL.Context.SetVertexBuffer(0, null);
				Director.Instance.GL.Context.SetShaderProgram(null);
				Director.Instance.GL.Context.SetFrameBuffer(old_frame_buffer);
				Director.Instance.GL.Context.SetScissor(old_scissor);
				Director.Instance.GL.Context.SetViewport(old_viewport);

				Console.WriteLine("SwapBuffers(): enter");
				Director.Instance.GL.Context.SwapBuffers();
				Console.WriteLine("SwapBuffers(): exit");
				Thread.Sleep(250);
			}

			public void Add(string name, Texture2D texture, int tiles_x, int tiles_y)
			{
				int tile_width = (int)FMath.Round((float)texture.Width / (float)tiles_x);
				int tile_height = (int)FMath.Round((float)texture.Height / (float)tiles_y);
				tile_width /= ScaleDivisor;
				tile_height /= ScaleDivisor;
				tile_width = System.Math.Max(1, tile_width);
				tile_height = System.Math.Max(1, tile_height);

				ColorBuffer color_buffer = new ColorBuffer(tile_width, tile_height, PixelFormat.Rgba);
				FrameBuffer frame_buffer = new FrameBuffer();
				frame_buffer.SetColorTarget(color_buffer);
				
				FrameBuffer old_frame_buffer = Director.Instance.GL.Context.GetFrameBuffer();
				Director.Instance.GL.Context.SetFrameBuffer(frame_buffer);

				ShaderProgram.SetAttributeBinding(0, "iPosition");
				ShaderProgram.SetAttributeBinding(1, "iUV");
				
				texture.SetWrap(TextureWrapMode.ClampToEdge);
				texture.SetFilter(TextureFilterMode.Linear);
				
				Director.Instance.GL.Context.SetTexture(0, texture);
				
				Director.Instance.GL.Context.SetVertexBuffer(0, VertexBuffer);
				Director.Instance.GL.Context.SetShaderProgram(ShaderProgram);
				
				ImageRect old_scissor = Director.Instance.GL.Context.GetScissor();
				ImageRect old_viewport = Director.Instance.GL.Context.GetViewport();
				
				Director.Instance.GL.Context.SetScissor(0, 0, tile_width, tile_height);
				Director.Instance.GL.Context.SetViewport(0, 0, tile_width, tile_height);
				
				Entry entry = new Entry();
				entry.TilesX = tiles_x;
				entry.TilesY = tiles_y;
				entry.TileWidth = tile_width;
				entry.TileHeight = tile_height;
				entry.Data = new List<List<byte>>();
				for (int i = 0; i < tiles_y * tiles_x; ++i)
					entry.Data.Add(new List<byte>());
				
				Vector2 uv_step = new Vector2(1.0f, 1.0f) / new Vector2(tiles_x, tiles_y);
				for (int y = 0; y < tiles_y; y++)
				{
					for (int x = 0; x < tiles_x; x++)
					{
						float uv_x0 = uv_step.X * (x + 0);
						float uv_x1 = uv_step.X * (x + 1);
						float uv_y0 = uv_step.Y * (tiles_y - 1 - y + 0);
						float uv_y1 = uv_step.Y * (tiles_y - 1 - y + 1);
						
						VertexDataArray[0] = new VertexData() { position = new Vector2(-1.0f, -1.0f), uv = new Vector2(uv_x0, uv_y1) };
						VertexDataArray[1] = new VertexData() { position = new Vector2(-1.0f, +1.0f), uv = new Vector2(uv_x0, uv_y0) };
						VertexDataArray[2] = new VertexData() { position = new Vector2(+1.0f, +1.0f), uv = new Vector2(uv_x1, uv_y0) };
						VertexDataArray[3] = new VertexData() { position = new Vector2(+1.0f, -1.0f), uv = new Vector2(uv_x1, uv_y1) };
						VertexBuffer.SetIndices(IndexDataArray, 0, 0, 4);
						VertexBuffer.SetVertices(VertexDataArray, 0, 0, 4);
						Director.Instance.GL.Context.SetVertexBuffer(0, VertexBuffer);
						
						Director.Instance.GL.Context.DrawArrays(DrawMode.TriangleFan, 0, 4);
						
						byte[] data = new byte[tile_width * tile_height * 4];

						// DEBUG: fill with visible memory pattern
						//for (int i = 0; i< tile_width * tile_height * 4; ++i)
							//data[i] = (byte)(i % (tile_width / 4));

						Director.Instance.GL.Context.ReadPixels(data, PixelFormat.Rgba, 0, 0, tile_width, tile_height);

						List<byte> output = entry.Data[tiles_x * y + x];
						for (int i = 0; i < tile_width * tile_height * 4; ++i)
							output.Add(data[i]);
					}
				}
				
				Director.Instance.GL.Context.SetVertexBuffer(0, null);
				Director.Instance.GL.Context.SetShaderProgram(null);
				Director.Instance.GL.Context.SetFrameBuffer(old_frame_buffer);
				Director.Instance.GL.Context.SetScissor(old_scissor);
				Director.Instance.GL.Context.SetViewport(old_viewport);

				TileData[name] = entry;
			}
			
			public Texture2D MakeTexture(string name, int tile)
			{
				Entry entry = TileData[name];
				List<byte> data = entry.Data[tile];
				
				Texture2D texture = new Texture2D(entry.TileWidth, entry.TileHeight, false, PixelFormat.Rgba);
				texture.SetPixels(0, data.ToArray());
				return texture;
			}
		}
		
		public static Vector4 Color(byte r, byte g, byte b, byte a = 255)
		{
			return new Sce.PlayStation.Core.Vector4(
				(float)r / 255.0f,
				(float)g / 255.0f,
				(float)b / 255.0f,
				(float)a / 255.0f
			);
		}
	}
}

