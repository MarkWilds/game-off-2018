using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Particles
{
    public class ParticleEmitter
    {
        private List<Particle> particles = new List<Particle>();
        private Random random;
        private int maxParticles;
        private Vector2 emitLocation;
        private bool repeat;
        private float particleLifeTime;
        private Vector2 particleDirection;
        private float particleScale;
        private ParticleShape particleShape;
        private Color startColor;
        private Color endColor;
        private float maxAngle;
        private float particleSpeed;
        private EmitType emitType;

        private float emitterLifeTime;
        private float particlesPerFrame;
        private bool paused = false;
        private Texture2D particleTexture;

        public bool ShouldBeDestroyed { get; private set; }

        /// <summary>
        /// Creates a new particleEmitter
        /// </summary>
        /// <param name="repeat">Should the emitter stay active and keep emitting particles after the max amount of particles have been emitted</param>
        /// <param name="maxParticles">Max amount of particles</param>
        /// <param name="emitLocation">The location to emit from</param>
        /// <param name="direction">The velocity of the particles</param>
        /// <param name="particleSpeed">The speed of the particles</param>
        /// <param name="maxAngle">The angle based on the velocity the particles can be emitted from</param>
        /// <param name="particleLifeTime">Lifetime of particles in seconds</param>
        /// <param name="particleScale">Size of particles </param>
        /// <param name="particleShape">Particle shape</param>
        /// <param name="startColor">Starting particle color</param>
        /// <param name="autoStart">Should the emitter start immediatly</param>
        /// <param name="emitType">The type of the emitter</param>
        /// <param name="endColor">The color of the particle at the end</param>
        public ParticleEmitter(bool autoStart, bool repeat, int maxParticles, Vector2 emitLocation, Vector2 direction, 
            float particleSpeed, float maxAngle, float particleLifeTime, float particleScale, ParticleShape particleShape, EmitType emitType, Color startColor, Color endColor)
        {
            this.repeat = repeat;
            this.maxParticles = maxParticles;
            this.emitLocation = emitLocation;
            this.particleDirection = direction;
            this.particleSpeed = particleSpeed;
            this.maxAngle = maxAngle;
            this.particleLifeTime = particleLifeTime;
            this.particleScale = particleScale;
            this.particleShape = particleShape;
            this.emitType = emitType;
            this.startColor = startColor;
            this.endColor = endColor;
            this.paused = !autoStart;
            random = new Random();

            if (repeat == false)
                emitterLifeTime = particleLifeTime;

            particleTexture = ParticleSystem.Instance.GetTexture(particleShape);
            ParticleSystem.Instance.AddEmitter(this);
        }

        public void Update(GameTime gameTime)
        {
            if (!repeat)
                emitterLifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            CreateNewParticles(gameTime);
            RemoveParticles(gameTime);

            if (repeat == false && emitterLifeTime <= 0)
                ShouldBeDestroyed = true;
        }

        private void RemoveParticles(GameTime gameTime)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update(gameTime);
                if (particles[i].lifeSpan <= 0)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Create new particles based on the emitType
        /// </summary>
        private void CreateNewParticles(GameTime gameTime)
        {
            if (paused)
                return;

            switch (emitType)
            {
                case EmitType.OverTime:
                        //Calculate how many particles per frame we should emit
                        particlesPerFrame = maxParticles * ((float)gameTime.ElapsedGameTime.TotalSeconds * particleLifeTime);

                        for (int i = 0; i < particlesPerFrame; i++)
                            particles.Add(GenerateNewParticle());
                    break;

                case EmitType.Burst:
                    for (int i = particles.Count; i < maxParticles; i++)
                        particles.Add(GenerateNewParticle());
                    break;
            }
        }

        private Particle GenerateNewParticle()
        {
            //Random angle based on emit direction and maxAngle;
            var randomAngle = (float)(random.NextDouble() * 2 - 1) * (maxAngle /2);
            var directionDeg = Math.Atan2(particleDirection.Y, particleDirection.X) * (180 / Math.PI) + randomAngle;
            var directionRad = directionDeg / (180 / Math.PI);

            //Calculate the new random direction from the direction and maxAngle
            var newDirection = new Vector2(
                (float)Math.Cos(directionRad),
                (float)Math.Sin(directionRad));

            //Randomize the speed
            var randomSpeed = random.NextDouble() * particleSpeed + particleSpeed / 2;

            return new Particle(particleTexture, emitLocation, newDirection, (float)randomSpeed, 0, 0, particleScale, particleLifeTime, startColor, endColor);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int index = 0; index < particles.Count; index++)
                particles[index].Draw(spriteBatch, gameTime);
        }

        public void Pause()
        {
            paused = false;
        }

        public void Resume()
        {
            paused = true;
        }

        public void Destroy()
        {
            particles.Clear();
        }
    }
}
