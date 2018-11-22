using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Particles
{
    public class Particle
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private float angle;
        private float angularVelocity;
        private Color startColor;
        private Color endColor;
        private float scale;
        public float lifeSpan;
        private float speed;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float particleSpeed, float angle, float angularVelocity, 
            float scale, float lifeSpan, Color startColor, Color endColor)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.startColor = startColor;
            this.scale = scale;
            this.lifeSpan = lifeSpan;
            this.speed = particleSpeed;
            this.endColor = endColor;
        }

        public void Update(GameTime gameTime)
        {
            lifeSpan -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity * (speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            angle += angularVelocity;
            startColor = Color.Lerp(startColor, endColor, (float)gameTime.ElapsedGameTime.TotalSeconds / lifeSpan);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, position, null, startColor, angle, origin, scale, SpriteEffects.None, 0);
        }
    }
}
