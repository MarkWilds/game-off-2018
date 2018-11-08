using System;
using game.GameScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    class Enemy : Entity
    {
        public float speed;
        private Vector2 direction;
        private float timeBetweenShots = 1;
        private float timer;
        private Entity target;

        public Enemy(float speed, Texture2D texture, Vector2 position, float rotation = 0)
            : base(texture, position, rotation)
        {
            this.speed = speed / 1000;
            this.EntityType = EntityType.Enemy;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (target == null)
                GetNewTarget();
            else
            {
                LookAtTarget();

                if (Vector2.Distance(position, target.position) < 300)
                    Shoot();
                else
                    MoveTowardsTarget(gameTime);
            }
        }

        private void MoveTowardsTarget(GameTime gameTime)
        {
            direction = target.position - position;
            position += direction * (speed * gameTime.ElapsedGameTime.Milliseconds);
        }

        private void Shoot()
        {
            if (timer < timeBetweenShots)
                return;

            EntityManager.Instance.AddEntity(new Bullet(OverworldScreen.BulletTexture, position, Forward, EntityType.Player, rotation));
            timer = 0;
        }

        private void LookAtTarget()
        {
            var direction = target.position - position;
            rotation = (float)Math.Atan2(direction.Y, direction.X) + ((1f * (float)Math.PI) / 2);
        }

        private void GetNewTarget()
        {
            target = EntityManager.Instance.GetEntitiesByType(EntityType.Player)[0];
        }
    }
}
