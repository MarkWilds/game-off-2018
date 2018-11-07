using game.Entities;
using game.GameScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class Player : Entity
    {
        private float speed;

        //TODO: Remove after testing
        public Texture2D bulletTexture;

        public Player(float speed, Texture2D texture, Vector2 position, float rotation = 0) 
            : base(texture, position, rotation)
        {
            this.speed = speed;
            EntityType = EntityType.Player;
            bulletTexture = OverworldScreen.BulletTexture;
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            LookAtMouse();
            Shoot();
        }

        private void Move(GameTime gameTime)
        {
            Vector2 direction = new Vector2();

            if (InputManager.IsKeyDown(Keys.A))
                direction.X -= 1;

            if (InputManager.IsKeyDown(Keys.D))
                direction.X += 1;

            if (InputManager.IsKeyDown(Keys.W))
                direction.Y -= 1;

            if (InputManager.IsKeyDown(Keys.S))
                direction.Y += 1;

            //Normalize vector to prevent faster movement when 2 directions are pressed
            if (direction.X != 0 || direction.Y != 0)
                direction.Normalize();

            Position += direction * (speed * gameTime.ElapsedGameTime.Milliseconds);
        }

        private void LookAtMouse()
        {
            var direction = Position - InputManager.MouseWorldPosition;
            Rotation = (float)Math.Atan2(direction.Y, direction.X) - ((1f * (float)Math.PI) / 2);
        }

        private void Shoot()
        {
            if (InputManager.MouseButtonClicked(MouseButton.Left))
                EntityManager.Instance.AddEntity(new Bullet(bulletTexture, sprite.Position, Forward, EntityType.Enemy, Rotation));
        }
    }
}
