using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    class Entity
    {
        protected Sprite sprite;

        /// <summary>
        /// If set to true, the entity will be removed from the game.
        /// </summary>
        public bool ShouldBeDestroyed { get; protected set; }

        /// <summary>
        /// The forward vector of the entity
        /// </summary>
        protected Vector2 Forward => new Vector2((float)Math.Sin(sprite.Rotation), -(float)Math.Cos(sprite.Rotation));

        //When entity position or rotation is set we adjust the sprite position and rotation
        public Vector2 Position { get { return sprite.Position; } set { sprite.Position = value; } }
        public float Rotation { get { return sprite.Rotation; } set { sprite.Rotation = value; } }

        public Entity(Texture2D texture, Vector2 position, float rotation = 0)
        {
            sprite = new Sprite(texture, position, rotation);
            Position = position;
            Rotation = rotation;

            EntityManager.AddEntity(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Destroy the entity deleting it from the game
        /// </summary>
        protected void Destroy()
        {
            ShouldBeDestroyed = true;
        }
    }
}
