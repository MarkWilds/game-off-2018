using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game.Particles;
using game.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game.Entities
{
    public class Car : Entity, IControllable
    {
        private float currentSpeed;
        private float topSpeed;
        private float acceleration;
        private float turnAngle;
        private bool started = false;
        private Player player;

        private Rectangle interactionBox;
        private ParticleEmitter exhaustParticles;
        private ParticleEmitter exhaustParticles2;
        private float interactionTimerCooldown = .5f;
        private SoundEffectWrapper carSound;

        public int Health { get; private set; } = 150;
        public int MaxHealth { get; private set; } = 150;

        public Car(float topSpeed, float acceleration, Texture2D texture, int width, int height, Vector2 position, float rotation = 0, Rectangle source = default(Rectangle)) 
            : base(texture, width, height, position, rotation, source)
        {
            this.topSpeed = topSpeed;
            this.acceleration = acceleration;
            turnAngle = 6f;

            exhaustParticles = new ParticleEmitter(false, true, 90, position, -Forward, .05f, 20, .45f, 0.25f * scale.X, ParticleShape.Circle, EmitType.OverTime, Color.Gray, Color.Transparent);
            exhaustParticles2 = new ParticleEmitter(false, true, 90, position, -Forward, .05f, 20, .45f, 0.25f * scale.X, ParticleShape.Circle, EmitType.OverTime, Color.Gray, Color.Transparent);

            player = EntityManager.Instance.GetPlayer() as Player;
            carSound = new SoundEffectWrapper("car", true, false, .05f, true);
        }

        public void Start()
        {
            started = true;
            player.playerController.ChangeControl(this);
            exhaustParticles.Start();
            exhaustParticles2.Start();
            interactionTimerCooldown = .5f;
            carSound.Play();
        }

        public void Stop()
        {
            started = false;
            player.playerController.ChangeControl(player);
            player.position = new Vector2(position.X + Width, position.Y + Height / 2);
            exhaustParticles.Stop();
            exhaustParticles2.Stop();
            interactionTimerCooldown = .5f;
            carSound.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            interactionTimerCooldown -= deltaTime;
            base.Update(gameTime);

            interactionBox = new Rectangle(BoundingBox.X - Width, BoundingBox.Y - Height, BoundingBox.Width * 2, BoundingBox.Height * 2);

            currentSpeed = Math.Clamp(currentSpeed, -topSpeed / 3, topSpeed);

            var direction = new Vector2(
                (float)Math.Cos(rotation),
                (float)Math.Sin(rotation));

            //Move car
            position += direction * currentSpeed * deltaTime;

            //If the player isn't driving the car, slow it down
            if (!started && currentSpeed != 0)
                SlowDown(deltaTime);

            //Set particleEmitter position
            exhaustParticles.SetLocation(position - Forward * Height * 0.98f + Right * Width / 6);
            exhaustParticles2.SetLocation(position - Forward * Height * 0.98f - Right * Width / 6);
            exhaustParticles.SetDirection(-Forward);
            exhaustParticles2.SetDirection(-Forward);

            if (currentSpeed >= 0)
                carSound.Pitch = (currentSpeed / topSpeed) - .75f;
            else if (currentSpeed < 0)
                carSound.Pitch = (currentSpeed / topSpeed) + 1 - .75f;

            Console.WriteLine(currentSpeed);
            carSound.Update(gameTime);
            
            CheckCollisions();
        }

        private void CheckCollisions()
        {
            if (player == null) return;

            if (interactionBox.Intersects(player.BoundingBox))
            {
                if (InputManager.IsKeyPressed(Keys.F) && !started && interactionTimerCooldown <= 0f)
                {
                    Start();
                }
            }

            if (currentSpeed < topSpeed - 100)
                return;

            var nearbyEnemies = EntityManager.Instance.GetDamageableEntities().Where(e => Vector2.Distance(position, e.position) < 10).ToArray();
            for (int i = 0; i < nearbyEnemies.Length; i++)
            {
                if (nearbyEnemies[i] == this)
                    return;

                if (nearbyEnemies[i].BoundingBox.Intersects(BoundingBox))
                {
                    ((IDamageable)nearbyEnemies[i]).TakeDamage((int)currentSpeed, Forward);
                }
            }
        }

        public void HandleInput(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputManager.IsKeyDown(Keys.W))
                currentSpeed += (acceleration * deltaTime);
            if (InputManager.IsKeyDown(Keys.S))
                currentSpeed -= (acceleration * 3 * deltaTime);

            if (InputManager.IsKeyDown(Keys.A))
                rotation -= (turnAngle * (currentSpeed / topSpeed) * deltaTime);
            if (InputManager.IsKeyDown(Keys.D))
                rotation += (turnAngle * (currentSpeed / topSpeed) * deltaTime);

            if (InputManager.IsKeyPressed(Keys.F) && started && interactionTimerCooldown <= 0f)
                Stop();

            //Not accelerating or braking
            if (!InputManager.IsKeyDown(Keys.W) && !InputManager.IsKeyDown(Keys.S))
                SlowDown(deltaTime);
        }

        public void TakeDamage(int amount, Vector2 hitDirection)
        {
            Health -= amount;
            if (Health <= 0)
            {
                if (started)
                    Stop();

                Destroy();
            }
        }

        public override void Destroy()
        {
            carSound.Stop();
            exhaustParticles.Destroy();
            SpawnExplosion();
            base.Destroy();
        }

        private void SpawnExplosion()
        {
            new ParticleEmitter(true, false, 50, position, Forward, .05f, 360, .75f, .5f, ParticleShape.Circle, EmitType.Burst, Color.DarkRed, Color.Transparent);
            new ParticleEmitter(true, false, 50, position, -Forward, .05f, 360, .75f, .5f, ParticleShape.Circle, EmitType.OverTime, Color.Black, Color.Transparent);
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
    }
}
