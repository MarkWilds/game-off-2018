﻿using game.Entities;
using game.Entities.Animations;
using game.GameScreens;
using game.Particles;
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

        private ParticleEmitter test;

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

            test = new ParticleEmitter(true, 50, position, -Forward, 5, 50, .25f, 2, ParticleShape.Square, Color.Aqua, Color.Aqua);
            test.Stop();
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
            if (InputManager.IsKeyDown(Keys.D1))
                test.Start();
            if (InputManager.IsKeyDown(Keys.D2))
                test.Stop();

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

        public void TakeDamage(int amount, Vector2 hitDirection)
        {
            Health -= amount;
            new ParticleEmitter(false, 25, position, -hitDirection, .05f, 180, .25f, 1, ParticleShape.Square, Color.Red, Color.Red);
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