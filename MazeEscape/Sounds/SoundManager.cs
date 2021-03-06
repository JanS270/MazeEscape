﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Sounds;

namespace MazeEscape.Sounds
{
    public class SoundManager
    {
        public Dictionary<string, SoundEffects> SoundEffects { get; set; }
        public Dictionary<string, BackgroundSongs> Songs { get; set; }

        private Dictionary<string, ISound> Active;

        private ContentManager Content;

        public SoundManager(ContentManager content, List<string> soundEffects = null, List<string> songs = null)
        {
            Content = content;
            SoundEffects = new Dictionary<string, SoundEffects>();
            Songs = new Dictionary<string, BackgroundSongs>();
            Active = new Dictionary<string, ISound>();
        }

        private bool IsUniqueName(KeyValuePair<string, string> key)
        {
            return !string.IsNullOrEmpty(key.Key.ToLower()) && !string.IsNullOrEmpty(key.Value) &&
                SoundEffects.Count(a => a.Key == key.Key.ToLower()) == 0 && Songs.Count(a => a.Key == key.Value) == 0;
        }

        public bool AddSong(KeyValuePair<string, string> song)
        {
            if (IsUniqueName(song))
            {
                var songFile = Content.Load<Song>(song.Value);
                Songs.Add(song.Key.ToLower(), new BackgroundSongs(songFile, false, 30f));
                return true;
            }
            return false;
        }

        public bool AddSound(KeyValuePair<string, string> song)
        {
            if (IsUniqueName(song))
            {
                var soundFile = Content.Load<SoundEffect>(song.Value);
                SoundEffects.Add(song.Key.ToLower(), new SoundEffects(soundFile));
                return true;
            }
            return false;
        }

        public Dictionary<string, string> AddSongs(Dictionary<string, string> songs)
        {
            var errorList = new Dictionary<string, string>();
            var error = false;

            foreach (var song in songs)
            {
                if (!AddSong(song))
                {
                    errorList.Add(song.Key, song.Value);
                }
            }

            return errorList;
        }

        public SoundEffect GetSoundEffect(string hoverSoundName)
        {
            return SoundEffects.FirstOrDefault(a => a.Key == hoverSoundName).Value.sound;
        }
        public Song GetSong(string hoverSoundName)
        {
            return Songs.FirstOrDefault(a => a.Key == hoverSoundName).Value.song;
        }

        public Dictionary<string, string> AddSounds(Dictionary<string, string> soundEffects)
        {
            var errorList = new Dictionary<string, string>();

            foreach (var soundEffect in soundEffects)
            {
                if (!AddSound(soundEffect))
                {
                    errorList.Add(soundEffect.Key, soundEffect.Value);
                }
            }

            return errorList;
        }

        public (Dictionary<string, string>, Dictionary<string, string>) Add(Dictionary<string, string> songs, Dictionary<string, string> soundEffects)
        {
            var errorSoundsList = AddSounds(soundEffects);
            var errorSongsList = AddSongs(songs);

            return (errorSongsList, errorSoundsList);
        }

        private ISound FindSound(string name)
        {
            var list = new List<ISound>();

            list.AddRange(SoundEffects.Where(a => a.Key == name).Select(a => a.Value).ToList());
            list.AddRange(Songs.Where(a => a.Key == name).Select(a => a.Value).ToList());

            return list.FirstOrDefault();
        }

        private ISound FindActiveSound(string name)
        {
            var list = Active.Where(a => a.Key == name).Select(a => a.Value).ToList();

            return list.FirstOrDefault();
        }

        public void Play(string name,bool replay = false)
        {
            var sound = FindSound(name);

            if (sound != null)
            {
                if (sound is BackgroundSongs song && replay)
                {
                    song.Repeating(true);
                }

                sound.Play();
                if (!Active.ContainsKey(name))
                {
                    Active.Add(name, sound);
                }
            }
        }

        public void Stop(string name)
        {
            var sound = FindActiveSound(name);

            if (sound != null)
            {
                sound.Stop();
                Active.Remove(name);
            }
        }

        public SoundType GetSound<SoundType>(string name) where SoundType : class
        {
            return FindSound(name) as SoundType;
        }

        // TODO Dodać Pause i Resume i Zmiane głośności

    }
}
