using game.GameScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    class Enemy : Entity
    {
        public float speed;
        private Vector2 direction;
        private float timeBetweenShots = 1;
        private float timer;
        private Vector2 target;

        public Enemy(float speed, Texture2D texture, Vector2 position, float rotation = 0)
            : base(texture, position, rotation)
        {
            this.speed = speed;
            this.EntityType = EntityTypes.Enemy;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void Shoot()
        {
            new Bullet(OverworldScreen.BulletTexture, Position, Forward, Rotation);
            timer = 0;
        }

        private void LookAtTarget()
        {
            var direction = target - Position;
            Rotation = (float)Math.Atan2(direction.Y, direction.X) + ((1f * (float)Math.PI) / 2);
        }

        private void BehaveLikeARetard()
        {
            LookAtTarget();

            if (timer >= timeBetweenShots)
                Shoot();
        }
    }
}
