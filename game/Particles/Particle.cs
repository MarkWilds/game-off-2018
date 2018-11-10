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
        private Color color;
        private float scale;
        public float lifeSpan;
        private float speed;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float particleSpeed, float angle, float angularVelocity, Color color, float scale, float lifeSpan)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.color = color;
            this.scale = scale;
            this.lifeSpan = lifeSpan;
            this.speed = particleSpeed;
        }

        public void Update(GameTime gameTime)
        {
            lifeSpan -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity * (speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            angle += angularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            spriteBatch.Draw(texture, position, sourceRectangle, color, angle, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
