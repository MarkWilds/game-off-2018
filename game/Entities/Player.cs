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
        private WeaponManager weaponManager;

        //TODO: Remove after testing
        public Texture2D bulletTexture;

        public Player(float speed, Texture2D texture, Vector2 position, float rotation = 0) 
            : base(texture, position, rotation)
        {
            this.speed = speed;
            EntityType = EntityType.Player;
            bulletTexture = OverworldScreen.BulletTexture;

            weaponManager = new WeaponManager(this);
            weaponManager.AddWeapon(new Pistol(OverworldScreen.BulletTexture, OverworldScreen.PistolTexture, Position + Forward));
            weaponManager.AddWeapon(new Pistol(OverworldScreen.BulletTexture, OverworldScreen.RifleTexture, Position + Forward));
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            weaponManager.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Shoot();

            weaponManager.Update(gameTime);
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

        private void Shoot()
        {
            if (InputManager.IsMouseButtonPressed(MouseButton.Left))
                weaponManager.CurrentWeapon.Shoot();
        }
    }
}
