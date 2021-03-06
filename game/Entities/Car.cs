﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game.Particles;
using game.Sound;
using game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game.Entities
{
    public class Car : Entity, IControllable
    {
        private bool IsPlayerDriving = false;
        private Player player;

        private Rectangle interactionBox => new Rectangle(BoundingBox.X - Width, BoundingBox.Y - Height, BoundingBox.Width * 2, BoundingBox.Height * 2);
        private ParticleEmitter exhaustParticles;
        private ParticleEmitter exhaustParticles2;
        private float interactionTimerCooldown = .5f;
        private SoundEffectWrapper carSound;

        public int Health { get; private set; } = 150;
        public int MaxHealth { get; private set; } = 150;
        private float currentSpeed;
        private float topSpeed = 500;
        private float acceleration = 75f;
        private float turnAngle = 6f;
        private Map map;

        public Car(Map map, Texture2D texture, int width, int height, Vector2 position, float rotation = 0, Rectangle source = default(Rectangle)) 
            : base(texture, width, height, position, rotation, source)
        {
            exhaustParticles = new ParticleEmitter(false, true, 90, position, -Forward, .05f, 20, .45f, 0.25f * scale.X, ParticleShape.Circle, EmitType.OverTime, Color.Gray, Color.Transparent);
            exhaustParticles2 = new ParticleEmitter(false, true, 90, position, -Forward, .05f, 20, .45f, 0.25f * scale.X, ParticleShape.Circle, EmitType.OverTime, Color.Gray, Color.Transparent);

            player = EntityManager.Instance.GetPlayer() as Player;
            carSound = new SoundEffectWrapper("car", true, false, .025f, true);
            this.map = map;
        }

        public void Start()
        {
            IsPlayerDriving = true;
            player.playerController.ChangeControl(this);
            exhaustParticles.Paused = false;
            exhaustParticles2.Paused = false;
            interactionTimerCooldown = .5f;
            carSound.Play();
        }

        public void ShutDown()
        {
            IsPlayerDriving = false;
            player.playerController.ChangeControl(player);
            player.position = new Vector2(position.X + Width, position.Y + Height / 2);
            exhaustParticles.Paused = true;
            exhaustParticles2.Paused = true;
            interactionTimerCooldown = .5f;
            carSound.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            interactionTimerCooldown -= deltaTime;

            HandleMovement(deltaTime);

            UpdateExhaustParticles();

            //Sound pitch based on currentSpeed
            if (currentSpeed >= 0)
                carSound.Pitch = (currentSpeed / topSpeed) - .5f;
            else if (currentSpeed < 0)
                carSound.Pitch = (currentSpeed / topSpeed);

            //Randomize the pitch of the sound to make it sound a bit more real
            carSound.Update(gameTime);

            CheckCollisions();
        }

        private void UpdateExhaustParticles()
        {
            exhaustParticles.Position = position - Forward * Height * 0.98f + Right * Width / 6;
            exhaustParticles2.Position = position - Forward * Height * 0.98f - Right * Width / 6;

            exhaustParticles.ParticleDirection = -Forward;
            exhaustParticles2.ParticleDirection = -Forward;
        }

        private void HandleMovement(float deltaTime)
        {
            currentSpeed = Math.Clamp(currentSpeed, -topSpeed / 3, topSpeed);

            var direction = new Vector2(
                (float)Math.Cos(rotation),
                (float)Math.Sin(rotation));

            if (!IsPlayerDriving && currentSpeed != 0)
                SlowDown(deltaTime);

            var velocity = direction * currentSpeed * deltaTime;
            velocity = map.Move(velocity, this);

            position += velocity;
        }

        private void CheckCollisions()
        {
            if (player == null) return;

            if (interactionBox.Intersects(player.BoundingBox))
            {
                if (InputManager.IsKeyPressed(Keys.F) && interactionTimerCooldown <= 0f)
                    Start();
            }

            if (currentSpeed < topSpeed - 100)
                return;

            //Get all nearby entities and check if we hit them
            var nearbyEntities = EntityManager.Instance.GetEntitiesInRange(position, 100);
            for (int i = 0; i < nearbyEntities.Length; i++)
            {
                if (nearbyEntities[i] == this)
                    return;

                if (nearbyEntities[i].BoundingBox.Intersects(BoundingBox))
                    ((IDamageable)nearbyEntities[i]).TakeDamage((int)currentSpeed, Forward);
            }
        }

        public void HandleInput(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentSpeed += InputManager.VerticalAxis * acceleration * deltaTime;
            rotation += InputManager.HorizontalAxis * turnAngle * (currentSpeed / topSpeed) * deltaTime;

            if (InputManager.IsKeyPressed(Keys.F) && interactionTimerCooldown <= 0f)
                ShutDown();

            //Not accelerating or braking
            if (InputManager.VerticalAxis == 0)
                SlowDown(deltaTime);
        }

        public void TakeDamage(int amount, Vector2 hitDirection)
        {
            Health -= amount;
            if (Health <= 0)
            {
                if (IsPlayerDriving)
                    ShutDown();

                new Explosion(this, 100, position);
                Destroy();
            }
        }

        private void SlowDown(float deltaTime)
        {
            if (currentSpeed > 0.1f)
                currentSpeed -= (acceleration * deltaTime);
            else if (currentSpeed < -0.1f)
                currentSpeed += (acceleration * deltaTime);

            //Stop the car if the currentspeed is really low
            if (currentSpeed <= 1f && currentSpeed >= -1f)
                currentSpeed = 0;
        }

        public override void Destroy()
        {
            carSound.Stop();
            exhaustParticles.Destroy();
            base.Destroy();
        }
    }
}
