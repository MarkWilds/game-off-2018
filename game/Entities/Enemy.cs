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
        private Entity target;
        private Vector2 direction;
        private float timeBetweenShots = 1;
        private float timer;

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

            if (target == null)
                SearchNewTarget();
            else
            {
                LookAtTarget();
                
                //Check if range to shoot
                if (Vector2.Distance(target.Position, Position) < 300)
                {
                    if(timer >= timeBetweenShots)
                        Shoot();
                }
                else
                {
                    direction = target.Position - Position;
                    Position += direction * (speed * gameTime.ElapsedGameTime.Milliseconds);
                }
            }
        }

        private void Shoot()
        {
            new Bullet(GameApplication.BulletTexture, Position, Forward, Rotation);
            timer = 0;
        }

        private void SearchNewTarget()
        {
            target = EntityManager.GetEntitiesByType(EntityTypes.Player)[0];
        }

        private void LookAtTarget()
        {
            var direction = target.Position - Position;
            Rotation = (float)Math.Atan2(direction.Y, direction.X) + ((1f * (float)Math.PI) / 2);
        }
    }
}
