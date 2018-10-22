﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Sounds
{
    // Klasa obsługująca efekty dzwiękowe
    public class SoundEffects
    {
        public SoundEffect sound { get; private set; } // Dzwiek do obsługi
        public string Name { get; }// Nazwa dzwięku
        public bool Wait { get; internal set; }

        ContentManager Content;

        // Dodawanie nowego dzwięku
        public SoundEffects(SoundEffect sound, ContentManager Content, string Name)
        {
            this.sound = sound;
            this.Name = Name;
        }

        // Wczytywanie nowego dzwięku z pliku
        public SoundEffects(string SongPath, ContentManager Content, string Name)
        {
            this.Content = Content;
            this.sound = Content.Load<SoundEffect>(SongPath);
            this.Name = Name;
        }

        // Zmiana dzwięku
        public void ChangeSound(string SongPath)
        {
            this.sound = Content.Load<SoundEffect>(SongPath);
        }

        // Odtwarzanie dzwięku
        public void Play()
        {
            if (!Wait)
            {
                sound.Play();
            }
        }
    }
}