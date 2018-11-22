using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Sound
{
    public class SoundEffectWrapper
    {
        private SoundEffectInstance soundEffectInstance;
        private Random random;
        private bool randPitch;

        public float Pitch { get { return soundEffectInstance.Pitch; } set { soundEffectInstance.Pitch = value; } }
        public float Pan { get { return soundEffectInstance.Pan; } set { soundEffectInstance.Pan = value; } }
        public float Volume { get { return soundEffectInstance.Volume; } set { soundEffectInstance.Volume = value; } }

        public SoundEffectWrapper(string soundName, bool looped, bool autoStart, float volume, bool randomizePitch)
        {
            this.randPitch = randomizePitch;
            random = new Random();

            soundEffectInstance = AudioManager.Instance.GetSoundEffectInstance(soundName, looped);
            this.Volume = volume;

            if (!autoStart) soundEffectInstance.Stop();
        }

        public void Update(GameTime gameTime)
        {
            RandomizePitch();
        }

        /// <summary>
        /// Randomize the pitch of the sound effect to give it a little more realism
        /// </summary>
        private void RandomizePitch()
        {
            if (Pitch >= 1.0f || Pitch <= -1.0f || !randPitch)
                return;

            Pitch = (float)random.Next(-5, 5) / 100 + Pitch;
        }

        public void Play()
        {
            soundEffectInstance.Play();
        }

        public void Resume()
        {
            soundEffectInstance.Resume();
        }

        public void Stop()
        {
            soundEffectInstance.Stop();
        }
    }
}
