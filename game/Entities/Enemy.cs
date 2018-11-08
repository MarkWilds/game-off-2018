using System;
using game.GameScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    class Enemy : Entity, IDamageable
    {
        private float speed;
        private float timeBetweenShots = 1;
        private float timer;
        private Entity target;
        private int damage = 10;

        public int Health { get; private set; } = 50;

        public Enemy(float speed, Texture2D texture, Vector2 position, float rotation = 0)
            : base(texture, position, rotation)
        {
            this.speed = speed;
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
            Vector2 direction = target.position - position;
            direction.Normalize();
            position += direction * speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void Shoot()
        {
            if (timer < timeBetweenShots)
                return;

            EntityManager.Instance.AddEntity(new Bullet(damage, OverworldScreen.BulletTexture, position + (Forward * Height), Forward, rotation));
            timer = 0;
        }

        private void LookAtTarget()
        {
            var direction = target.position - position;
            rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        private void GetNewTarget()
        {
            target = EntityManager.Instance.GetPlayer();
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
    }
}
