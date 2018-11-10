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
        public float EmitterLifeTime { get; private set; }
        private float particleLifeTime;
        private Vector2 particleVelocity;
        private float particleScale;
        private ParticleShape particleShape;
        private Color color;
        private float maxAngle;
        private float particleSpeed;

        public ParticleEmitter(float emitterLifeTime, int maxParticles, Vector2 emitLocation, Vector2 particleVelocity, 
            float particleSpeed, float maxAngle, float particleLifeTime, float particleScale, ParticleShape particleShape, Color color)
        {
            this.maxParticles = maxParticles;
            this.emitLocation = emitLocation;
            this.particleLifeTime = particleLifeTime;
            this.particleVelocity = particleVelocity;
            this.particleScale = particleScale;
            this.particleShape = particleShape;
            this.color = color;
            this.maxAngle = maxAngle;
            this.particleSpeed = particleSpeed;
            EmitterLifeTime = emitterLifeTime;
            random = new Random();

            ParticleSystem.Instance.AddEmitter(this);
        }

        public void Update(GameTime gameTime)
        {
            EmitterLifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = particles.Count; i < maxParticles; i++)
            {
                particles.Add(GenerateNewParticle());
            }

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

        private Particle GenerateNewParticle()
        {
            Texture2D texture = ParticleSystem.Instance.GetTexture(particleShape);

            var randomAngle = (float)(random.NextDouble() * 2 - 1) * maxAngle;
            var directionDeg = Math.Atan2(particleVelocity.Y, particleVelocity.X) * (180 / Math.PI) + randomAngle;
            var directionRad = directionDeg / (180 / Math.PI);

            var newDirection = new Vector2(
                (float)Math.Cos(directionRad),
                (float)Math.Sin(directionRad));

            var randomSpeed = random.NextDouble() * particleSpeed + particleSpeed / 2;

            return new Particle(texture, emitLocation, newDirection, (float)randomSpeed, 0, 0, color, particleScale, particleLifeTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch, gameTime);
            }
        }
    }
}
