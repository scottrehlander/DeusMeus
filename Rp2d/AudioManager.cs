using System;
using Sce.PlayStation.Core.Audio;
using System.Collections.Generic;

namespace Rp2d
{
	public static class AudioManager
	{
		
		private static BgmPlayer _bgmPlayer;
		private static Bgm _currentBackgroundMusic;
		
		private static Dictionary<string, SoundPlayer> _soundPlayers;
		
		
		public static void PlayBackgroundSong(string filename)
		{
			// Set the current background music
			if(_currentBackgroundMusic != null)
			{
				_currentBackgroundMusic.Dispose();
			}
			
			_currentBackgroundMusic = new Bgm(filename);
			
			if(_bgmPlayer != null)
			{
				_bgmPlayer.Dispose();
			}
			
			// Create the player and play the background music
			_bgmPlayer = _currentBackgroundMusic.CreatePlayer();
			_bgmPlayer.Play();
		}
		
		public static void PrecacheSound(string key, string soundFilename)
		{
			if(_soundPlayers == null)
			{
				_soundPlayers = new Dictionary<string, SoundPlayer>();
			}
			
			// Sound is already cached
			if(_soundPlayers.ContainsKey(key))
			{
				return;
			}
			
			Sound sound = new Sound(soundFilename);
			_soundPlayers.Add(key, sound.CreatePlayer());
		}
		
		public static void PlayPrecachedSound(string filename)
		{
			if(_soundPlayers == null)
			{
				_soundPlayers = new Dictionary<string, SoundPlayer>();
			}
			
			// If the sound is not precacahed, cache it
			if(!_soundPlayers.ContainsKey(filename))
			{
				Sound sound = new Sound(filename);
				_soundPlayers.Add(filename, sound.CreatePlayer());
				
#if DEBUG
				Console.Out.WriteLine("Sound was not precached, creating it now");
#endif
			}
			
			_soundPlayers[filename].Play();
		}
		
		public static void RemoveAllSounds()
		{
			foreach(var player in _soundPlayers)
			{
				player.Value.Dispose();
			}
			
			_soundPlayers.Clear();
		}
		
	}
	
}

