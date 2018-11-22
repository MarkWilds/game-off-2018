using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace game.Sound
{
    public class AudioManager
    {
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new AudioManager();
                return instance;
            }
        }
        private static AudioManager instance;
        private AudioManager() {}

        public void Initialize(ContentManager contentManager)
        {
            var audioDir = new DirectoryInfo(Path.Combine(contentManager.RootDirectory, @"Audio"));
            var audioFiles = audioDir.GetFiles("*.xnb");

            soundEffects = new Dictionary<string, SoundEffect>();

            for (int i = 0; i < audioFiles.Length; i++)
            {
                var soundName = Path.GetFileNameWithoutExtension(audioFiles[i].Name);
                soundEffects[soundName] = contentManager.Load<SoundEffect>("Audio/" + soundName);
                soundEffects[soundName].Name = soundName;
            }
        }

        private Dictionary<string, SoundEffect> soundEffects;

        public void PlaySoundEffect(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                soundEffects[soundName].Play(0.1f, 0, 0);
            }
        }

        public SoundEffectInstance GetSoundEffectInstance(string soundName, bool looped)
        {
            SoundEffectInstance effectInstance = null;
            if (soundEffects.ContainsKey(soundName))
            {
                effectInstance = soundEffects[soundName].CreateInstance();
                if (effectInstance != null)
                {
                    effectInstance.IsLooped = looped;
                    effectInstance.Play();
                }
            }

            return effectInstance;
        }
    }
}
