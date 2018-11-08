using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    public class Entity
    {
        /// <summary>
        /// If set to true, the entity will be removed from the game.
        /// </summary>
        public bool ShouldBeDestroyed { get; protected set; }

        public EntityType EntityType { get; protected set; }

        /// <summary>
        /// The forward vector of the entity
        /// </summary>
        public Vector2 Forward => new Vector2((float) Math.Cos(rotation), (float) Math.Sin(rotation));

        public Vector2 position;
        public float rotation;
        
        private Texture2D texture;
        
        public int Height => texture.Height;
        public int Width => texture.Width;
        public Vector2 Center => new Vector2(position.X - Width / 2, position.Y - Height / 2);
        public Rectangle BoundingBox => new Rectangle((int) Center.X, (int) Center.Y, Width, Height);

        public Entity(Texture2D texture, Vector2 position, float rotation = 0)
        {
            this.texture = texture;
            this.position = position;
            this.rotation = rotation;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation + (float) Math.PI / 2,
                new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Destroy the entity deleting it from the game
        /// </summary>
        public void Destroy()
        {
            ShouldBeDestroyed = true;
        }
    }
}