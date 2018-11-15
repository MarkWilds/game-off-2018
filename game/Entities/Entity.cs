using System;
using game.Entities.Animations;
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

        /// <summary>
        /// The forward vector of the entity
        /// </summary>
        public Vector2 Forward => new Vector2((float) Math.Cos(rotation), (float) Math.Sin(rotation));

        public Vector2 position { get; set; }
        public float rotation;

        protected Animator animator;
        private Texture2D texture;
        
        public int Height;
        public int Width;
        public Vector2 Center => new Vector2(position.X - Width / 2, position.Y - Height / 2);
        public Rectangle BoundingBox => IsVisible ? new Rectangle((int)Center.X, (int)Center.Y, Width, Height) : new Rectangle();
        protected bool IsVisible = true;

        public Entity(Texture2D texture, int width, int height, Vector2 position, float rotation = 0)
        {
            this.texture = texture;
            this.position = position;
            this.rotation = rotation;
            this.Width = width;
            this.Height = height;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsVisible)
                return;

            var sourceRect = new Rectangle(0,0, Width, Height);
            if(animator != null)
            {
                var framePos = animator.FramePosition;
                sourceRect.X = (int)framePos.X;
                sourceRect.Y = (int)framePos.Y;
            }

            spriteBatch.Draw(texture, position, sourceRect, Color.White, rotation,
                new Vector2(Width / 2, Height / 2), 1, SpriteEffects.None, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsVisible)
                return;

            if (animator != null)
                animator.Update(gameTime);
        }

        /// <summary>
        /// Destroy the entity deleting it from the game
        /// </summary>
        public virtual void Destroy()
        {
            ShouldBeDestroyed = true;
        }
    }
}