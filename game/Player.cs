using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class Player
    {
        private Sprite sprite;
        private float speed;

        public Player(Sprite sprite, float speed)
        {
            this.sprite = sprite;
            this.speed = speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 movement = new Vector2();
            float deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            if (InputManager.IsKeyPressed(Keys.A))
                movement.X -= 1;

            if (InputManager.IsKeyPressed(Keys.D))
                movement.X += 1;

            if (InputManager.IsKeyPressed(Keys.W))
                movement.Y -= 1;

            if (InputManager.IsKeyPressed(Keys.S))
                movement.Y += 1;

            //Normalize vector to prevent faster movement when 2 directions are pressed
            if(movement.X != 0|| movement.Y != 0)
                movement.Normalize();

            sprite.Position += movement * (speed * deltaTime);
        }
    }
}
