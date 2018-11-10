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
        private Vector2 particleVelocity;
        private float particleScale;
        private ParticleShape particleShape;
        private Color startColor;
        private Color endColor;
        private float maxAngle;
        private float particleSpeed;
        private EmitType emitType;

        private float emitterLifeTime;
        private float timeTillNextParticle;
        private bool shouldEmit = true;

        public bool ShouldBeDestroyed { get; private set; }

        /// <summary>
        /// Creates a new particleEmitter
        /// </summary>
        /// <param name="repeat">Should the emitter stay active and keep emitting particles after the max amount of particles have been emitted</param>
        /// <param name="maxParticles">Max amount of particles</param>
        /// <param name="emitLocation">The location to emit from</param>
        /// <param name="particleVelocity">The velocity of the particles</param>
        /// <param name="particleSpeed">The speed of the particles</param>
        /// <param name="maxAngle">The angle based on the velocity the particles can be emitted from</param>
        /// <param name="particleLifeTime">Lifetime of particles in seconds</param>
        /// <param name="particleScale">Size of particles </param>
        /// <param name="particleShape">Particle shape</param>
        /// <param name="startColor">Starting particle color</param>
        /// <param name="autoStart">Should the emitter start immediatly</param>
        /// <param name="emitType">The type of the emitter</param>
        /// <param name="endColor">The color of the particle at the end</param>
        public ParticleEmitter(bool autoStart, bool repeat, int maxParticles, Vector2 emitLocation, Vector2 particleVelocity, 
            float particleSpeed, float maxAngle, float particleLifeTime, float particleScale, ParticleShape particleShape, EmitType emitType, Color startColor, Color endColor)
        {
            this.repeat = repeat;
            this.maxParticles = maxParticles;
            this.emitLocation = emitLocation;
            this.particleVelocity = particleVelocity;
            this.particleSpeed = particleSpeed;
            this.maxAngle = maxAngle;
            this.particleLifeTime = particleLifeTime;
            this.particleScale = particleScale;
            this.particleShape = particleShape;
            this.emitType = emitType;
            this.startColor = startColor;
            this.endColor = endColor;
            this.shouldEmit = autoStart;
            random = new Random();

            if (repeat == false)
                emitterLifeTime = particleLifeTime;

            if (emitType == EmitType.OverTime)
                timeTillNextParticle = particleLifeTime / maxParticles;

            ParticleSystem.Instance.AddEmitter(this);
        }

        public void Update(GameTime gameTime)
        {
            if (!repeat)
                emitterLifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            CreateNewParticles(gameTime);

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update(gameTime);
                if (particles[i].lifeSpan <= 0)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }

            if (repeat == false && emitterLifeTime <= 0)
                ShouldBeDestroyed = true;
        }

        private void CreateNewParticles(GameTime gameTime)
        {
            if (!shouldEmit)
                return;

            switch (emitType)
            {
                case EmitType.OverTime:
                    timeTillNextParticle -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(timeTillNextParticle <= 0)
                    {
                        particles.Add(GenerateNewParticle());
                        timeTillNextParticle = particleLifeTime / maxParticles; 
                    }
                    break;

                case EmitType.Burst:
                    for (int i = particles.Count; i < maxParticles; i++)
                    {
                        particles.Add(GenerateNewParticle());
                    }
                    break;
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = ParticleSystem.Instance.GetTexture(particleShape);

            var randomAngle = (float)(random.NextDouble() * 2 - 1) * (maxAngle /2);
            var directionDeg = Math.Atan2(particleVelocity.Y, particleVelocity.X) * (180 / Math.PI) + randomAngle;
            var directionRad = directionDeg / (180 / Math.PI);

            var newDirection = new Vector2(
                (float)Math.Cos(directionRad),
                (float)Math.Sin(directionRad));

            var randomSpeed = random.NextDouble() * particleSpeed + particleSpeed / 2;

            return new Particle(texture, emitLocation, newDirection, (float)randomSpeed, 0, 0, particleScale, particleLifeTime, startColor, endColor);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch, gameTime);
            }
        }

        public void Stop()
        {
            shouldEmit = false;
        }

        public void Start()
        {
            shouldEmit = true;
        }

        public void Destroy()
        {
            particles.Clear();
        }
    }
}
