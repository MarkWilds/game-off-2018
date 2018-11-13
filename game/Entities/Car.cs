using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game.Entities
{
    class Car : Entity
    {
        private float currentSpeed;
        private float topSpeed;
        private float acceleration;
        private float steeringSpeed;

        public Car(float topSpeed, float acceleration, Texture2D texture, int width, int height, Vector2 position, float rotation = 0) : base(texture, width, height, position, rotation)
        {
            this.topSpeed = topSpeed;
            this.acceleration = acceleration;
            steeringSpeed = .004f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            if (InputManager.IsKeyDown(Keys.W))
                currentSpeed += (acceleration * deltaTime);
            if (InputManager.IsKeyDown(Keys.A))
                rotation -= (steeringSpeed * (currentSpeed / topSpeed) * deltaTime);
            if (InputManager.IsKeyDown(Keys.S))
                currentSpeed -= (acceleration * 3 * deltaTime);
            if (InputManager.IsKeyDown(Keys.D))
                rotation += (steeringSpeed * (currentSpeed / topSpeed) * deltaTime);

            //Not accelerating or braking
            if (!InputManager.IsKeyDown(Keys.W) && !InputManager.IsKeyDown(Keys.S)) {
                if (currentSpeed > 0)
                    currentSpeed -= (acceleration * deltaTime / 2);
                else if (currentSpeed < 0)
                    currentSpeed += (acceleration * deltaTime / 2);

                //Stop the car if the currentspeed is really low
                if (currentSpeed <= 0.1f && currentSpeed >= -0.1f)
                    currentSpeed = 0;
            }

            currentSpeed = Math.Clamp(currentSpeed, -topSpeed / 2, topSpeed);

            var direction = new Vector2(
                (float)Math.Cos(rotation),
                (float)Math.Sin(rotation));

            position += direction * currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine(currentSpeed);
        }
    }
}
