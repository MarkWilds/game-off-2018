using System;
using System.Collections.Generic;
using System.Text;
using game.Particles;
using Microsoft.Xna.Framework;
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
        private bool isOn = false;
        private Player player;

        private Rectangle interactionBox;
        private ParticleEmitter exhaustParticles;
        private Vector2 exhaustOffset;

        public int Health { get; private set; } = 150;

        public Car(float topSpeed, float acceleration, Texture2D texture, int width, int height, Vector2 position, float rotation = 0) : base(texture, width, height, position, rotation)
        {
            this.topSpeed = topSpeed;
            this.acceleration = acceleration;
            turnAngle = .004f;

            exhaustOffset = new Vector2(0, height);
            exhaustParticles = new ParticleEmitter(false, true, 90, position + exhaustOffset, -Forward, .05f, 20, .45f, .4f, ParticleShape.Circle, EmitType.OverTime, Color.Gray, Color.Transparent);

            player = EntityManager.Instance.GetPlayer() as Player;
        }

        public void Enter()
        {
            Console.WriteLine("Enter");
            isOn = true;
            player.playerController.ChangeControl(this);
            exhaustParticles.Start();
        }

        public void Exit()
        {
            Console.WriteLine("Exit");
            isOn = false;
            player.playerController.ChangeControl(player);
            player.position = new Vector2(position.X + Width, position.Y + Height / 2);
            exhaustParticles.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            interactionBox = new Rectangle(BoundingBox.X - 1, BoundingBox.Y - 1, BoundingBox.Width + 2, BoundingBox.Y + 2);

            currentSpeed = Math.Clamp(currentSpeed, -topSpeed / 2, topSpeed);

            var direction = new Vector2(
                (float)Math.Cos(rotation),
                (float)Math.Sin(rotation));

            //Move car
            position += direction * currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Set particleEmitter position
            exhaustParticles.SetLocation(position);
            exhaustParticles.SetDirection(-Forward);

            CheckTrigger();
        }

        private void CheckTrigger()
        {
            if (player == null) return;

            if (interactionBox.Intersects(player.BoundingBox))
            {
                if (InputManager.IsKeyPressed(Keys.F) && !isOn)
                {
                    Enter();
                }
            }
        }

        public void HandleInput(GameTime gameTime)
        {
            var deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            if (InputManager.IsKeyDown(Keys.W))
                currentSpeed += (acceleration * deltaTime);
            if (InputManager.IsKeyDown(Keys.A))
                rotation -= (turnAngle * (currentSpeed / topSpeed) * deltaTime);
            if (InputManager.IsKeyDown(Keys.S))
                currentSpeed -= (acceleration * 3 * deltaTime);
            if (InputManager.IsKeyDown(Keys.D))
                rotation += (turnAngle * (currentSpeed / topSpeed) * deltaTime);

            if (InputManager.IsKeyPressed(Keys.F))
            {
                Exit();
            }

            //Not accelerating or braking
            if (!InputManager.IsKeyDown(Keys.W) && !InputManager.IsKeyDown(Keys.S))
            {
                if (currentSpeed > 0)
                    currentSpeed -= (acceleration * deltaTime / 2);
                else if (currentSpeed < 0)
                    currentSpeed += (acceleration * deltaTime / 2);

                //Stop the car if the currentspeed is really low
                if (currentSpeed <= 0.1f && currentSpeed >= -0.1f)
                    currentSpeed = 0;
            }
        }

        public void TakeDamage(int amount, Vector2 hitDirection)
        {
            Health -= amount;
            if (Health <= 0)
            {
                Exit();
                Destroy();
            }
        }

        public override void Destroy()
        {
            exhaustParticles.Destroy();
            base.Destroy();
        }
    }
}
