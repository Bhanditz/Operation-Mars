﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace coolgame
{
    public static class SoundManager
    {
        private static Dictionary<string, SoundEffect> clips = new Dictionary<string, SoundEffect>();
        private static List<Song> songs = new List<Song>();

        private static Song menuMusic;
        public static bool MusicMuted;
        public static bool SoundMuted;

        private static float soundVolume;
        public static int SoundVolume
        {
            get { return (int)soundVolume; }
            set
            {
                soundVolume = value/100f;
                if(!MusicMuted)
                {
                    SoundEffect.MasterVolume = soundVolume;
                }
            }
        }

        private static float musicVolume;
        public static int MusicVolume
        {
            get { return (int)musicVolume; }
            set
            {
                musicVolume = value/100f;
                if (!MusicMuted)
                {
                    MediaPlayer.Volume = musicVolume;
                }
            }
        }

        public static void MuteMusic()
        {
            if (!MusicMuted)
            {
                MediaPlayer.Volume = 0;
                MusicMuted = true;
            }
            else
            {
                MediaPlayer.Volume = musicVolume;
                MusicMuted = false;
            }
        }

        public static void MuteSound()
        {
            if (!SoundMuted)
            {
                SoundEffect.MasterVolume = 0;
                SoundMuted = true;
            }
            else
            {
                SoundEffect.MasterVolume = soundVolume;
                SoundMuted = false;
            }
        }

        public static void AddClip(SoundEffect clip, string name)
        {
            clips.Add(name, clip);
        }

        public static void AddSong(Song song)
        {
            songs.Add(song);
        }

        public static void LoadContent(ContentManager Content)
        {
            AddSong(Content.Load<Song>("kaliope"));
            AddSong(Content.Load<Song>("cosmicMessages"));
            AddSong(Content.Load<Song>("Building-Itself"));
            AddSong(Content.Load<Song>("Cosmic-Switchboard"));
            AddSong(Content.Load<Song>("Dystopic-Technology"));
            AddSong(Content.Load<Song>("Restricted-Zone"));
            AddSong(Content.Load<Song>("Runaway-Technology"));
            menuMusic = Content.Load<Song>("mainMenu");

            AddClip(Content.Load<SoundEffect>("button1"), "button1");
            AddClip(Content.Load<SoundEffect>("button2"), "button2");

            AddClip(Content.Load<SoundEffect>("towerlaser"), "enemylaser");
            AddClip(Content.Load<SoundEffect>("towerlaser2"), "laser");
            AddClip(Content.Load<SoundEffect>("crawlerhit"), "crawlerhit");
            AddClip(Content.Load<SoundEffect>("steelroachhit"), "steelroachhit");
            AddClip(Content.Load<SoundEffect>("steelroachattack"), "steelroachattack");
            AddClip(Content.Load<SoundEffect>("reptilianattack"), "reptilianattack");
            AddClip(Content.Load<SoundEffect>("mwathit"), "mwathit");
            AddClip(Content.Load<SoundEffect>("ebloop"), "electrobeam");
            AddClip(Content.Load<SoundEffect>("metalrobothit"), "metalrobothit");

            MediaPlayer.MediaStateChanged += ChangeSong;
        }

        private static void ChangeSong(object sender, EventArgs e)
        {
            if(GameManager.State != GameState.StartMenu)
            {
                MediaPlayer.Play(songs[GameManager.RNG.Next(0, songs.Count)]);
            }
        }

        public static void PlayClip(string clipName)
        {
            if(!SoundMuted)
            {
                clips[clipName].Play();
            }
        }

        public static void PlayMenuMusic()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(menuMusic);
        }

        public static void PlayMusic()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsShuffled = true;

            MediaPlayer.Stop();
            MediaPlayer.Play(songs[GameManager.RNG.Next(0, songs.Count)]);
        }

        public static void PauseMusic()
        {
            MediaPlayer.Pause();
        }

        public static void ResumeMusic()
        {
            MediaPlayer.Resume();
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
