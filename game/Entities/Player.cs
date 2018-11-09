using game.Entities;
using game.Entities.Animations;
using game.GameScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class Player : Entity, IDamageable
    {
        private float speed;
        public WeaponManager WeaponManager { get; private set; }

        public int MaxHealth { get; private set; } = 100;
        public int Health { get; private set; }

        public Player(float speed, Texture2D texture, Vector2 position, float rotation = 0)
            : base(texture, 32, 32, position, rotation)
        {
            this.speed = speed;
            Health = MaxHealth;

            animator = new Animator(32, 32);
            animator.AddAnimation(new Animation(0, 1, 0)); //Idle
            animator.AddAnimation(new Animation(1, 3, 100)); //Running

            WeaponManager = new WeaponManager(this);
            WeaponManager.AddWeapon(new Pistol(OverworldScreen.BulletTexture, OverworldScreen.PistolTexture,
                base.position + Forward));
            WeaponManager.AddWeapon(new AssaultRifle(OverworldScreen.BulletTexture, OverworldScreen.RifleTexture,
                base.position + Forward));
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            WeaponManager.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Shoot();

            WeaponManager.Update(gameTime);
            base.Update(gameTime);
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

            if (direction.X != 0 || direction.Y != 0)
                animator.ChangeAnimation(1);
            else
                animator.ChangeAnimation(0);

            //Testing for ammo
            if (InputManager.IsKeyPressed(Keys.D1))
                WeaponManager.AddAmmo(Weapons.BulletType.Pistol, 30);
            if (InputManager.IsKeyPressed(Keys.D2))
                WeaponManager.AddAmmo(Weapons.BulletType.AssaultRifle, 30);

            //Normalize vector to prevent faster movement when 2 directions are pressed
            if (direction.X != 0 || direction.Y != 0)
                direction.Normalize();

            position += direction * speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void Shoot()
        {
            if (InputManager.IsMouseButtonPressed(MouseButton.Left))
                WeaponManager.ShootCurrentWeapon();
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            Destroy();
        }

        public bool Heal(int amount)
        {
            if (Health == MaxHealth)
                return false;

            Health += amount;
            if (Health > MaxHealth)
                Health = MaxHealth;

            return true;
        }
    }
}