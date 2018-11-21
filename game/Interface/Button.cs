using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Interface
{
    public class Button
    {
        private SpriteFont font;
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public string Text { get; private set; }
        private Color textColor;
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, 200, 50);

        public event EventHandler OnClick;
        private bool isHovered;

        public Button(SpriteFont font, Texture2D texture, Vector2 position, string text, Color textColor)
        {
            this.font = font;
            this.Texture = texture;
            this.Position = position;
            this.Text = text;
            this.textColor = textColor;

            Position -= new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 2);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var textureColor = Color.White;

            if (isHovered)
                textureColor = Color.Gray;

            spriteBatch.Draw(Texture, BoundingBox, textureColor);

            if(!string.IsNullOrEmpty(Text))
            {
                var x = (BoundingBox.X + (BoundingBox.Width / 2)) - (font.MeasureString(Text).X / 2);
                var y = (BoundingBox.Y + (BoundingBox.Height / 2)) - (font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(font, Text, new Vector2(x, y), textColor);
            }
        }

        public void Update(GameTime gameTime)
        {
            var mouseRect = new Rectangle((int)InputManager.MousePosition.X, (int)InputManager.MousePosition.Y, 1, 1);

            isHovered = false;

            if (mouseRect.Intersects(BoundingBox))
            {
                isHovered = true;

                if (InputManager.MouseButtonClicked(MouseButton.Left))
                    OnClick?.Invoke(this, new EventArgs());
            }
        }
    }
}
