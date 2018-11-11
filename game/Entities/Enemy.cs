using System;
using game.Core;
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
        private Player player;
        private Map map;

        public int Health { get; private set; } = 50;

        public Enemy(float speed, Texture2D texture, Vector2 position, Player player, Map map)
            : base(texture, 32, 32, position, 0)
        {
            this.speed = speed;
            this.player = player;
            this.map = map;
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
                if (Vector2.Distance(position, target.position) < 10)
                    Shoot();
                else
                    MoveTowardsTarget(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            RenderPath(spriteBatch);
        }

        private void RenderPath(SpriteBatch spriteBatch)
        {
            var path = map.GetPath(tilePosition, player.tilePosition);
            var lastNode = position;
            foreach(var node in path)
            {
                spriteBatch.DrawLine(node, lastNode, Color.Red);
                lastNode = node;
            }
        }

        private void MoveTowardsTarget(GameTime gameTime)
        {
            var path = map.GetPath(tilePosition, player.tilePosition);
            if (path.Count > 1)
            {
                Vector2 direction = path[1] - position;
                direction.Normalize();
                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
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
