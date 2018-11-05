using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class Sprite
    {
        private Texture2D texture;
        public Vector2 Position;
        public float Rotation;
        private float scale;

        public Sprite(Texture2D texture, Vector2 position, float rotation = 0, float scale = 1)
        {
            this.texture = texture;
            this.Position = position;
            this.Rotation = rotation;
            this.scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
        }
    }
}
