using game.Core;
using game.GameScreens;
using game.Particles;
using game.Sound;
using game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace game.Entities
{
    public class Enemy : Entity, IDamageable, IScout
    {
        private float timeBetweenShots = 1;
        private float timer;
        private IDamageable target => EntityManager.Instance.GetPlayer();
        private int damage = 10;
        private Map map;

        private bool hasRunningPathRequest = false;
        private Queue<Vector2> path;

        public int Health { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;
        private float speed = 128;

        public Enemy(Texture2D texture, Vector2 position, int width, int height, Map map, float rotation = 0, Rectangle source = default(Rectangle))
            : base(texture, width, height, position, rotation, source)
        {
            this.map = map;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer += (float) gameTime.ElapsedGameTime.TotalSeconds;

            LookAtTarget();

            if (Vector2.Distance(position, target.position) < 200)
                Shoot();
            else
            {
                if(path != null && path.Count > 1)
                    MoveTowardsTarget(gameTime);

                if (hasRunningPathRequest)
                    return;

                else if(target != null && Vector2.Distance(position, target.position) < 300)
                {
                    map.RequestPath(this, tilePosition, ((Entity)target).tilePosition);
                    hasRunningPathRequest = true;
                }
            }
                
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            return;
            if (path == null)
                return;

            RenderPath(spriteBatch);
        }

        private void RenderPath(SpriteBatch spriteBatch)
        {
            //var path = map.GetPath(tilePosition, ((Entity)target).tilePosition);
            var lastNode = position;
            foreach (var node in path)
            {
                spriteBatch.DrawLine(node, lastNode, Color.Red);
                lastNode = node;
            }
        }

        private void MoveTowardsTarget(GameTime gameTime)
        {
            if (path.Count > 0)
            {
                var point = path.Peek();
                Vector2 direction = point - position;
                direction.Normalize();
                position += direction * speed * (float) gameTime.ElapsedGameTime.TotalSeconds;

                if (Vector2.Distance(position, point) < 5)
                    path.Dequeue();
            }
        }

        private void Shoot()
        {
            if (timer < timeBetweenShots)
                return;

            EntityManager.Instance.AddEntity(new Bullet(damage, OverworldScreen.BulletTexture,
                position + (Forward * Height), Forward, rotation));
            timer = 0;
            AudioManager.Instance.PlaySoundEffect("GunShot");
        }

        private void LookAtTarget()
        {
            var direction = target.position - position;
            rotation = (float) Math.Atan2(direction.Y, direction.X);
        }

        public void TakeDamage(int amount, Vector2 hitDirection)
        {
            Health -= amount;
            new ParticleEmitter(true, false, 25, position, -hitDirection, .05f, 180, .25f, 1, ParticleShape.Square,
                EmitType.Burst, Color.Red, Color.Red);

            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            Destroy();
        }

        public void RecievePath(Queue<Vector2> path)
        {
            hasRunningPathRequest = false;
            this.path = path;
        }
    }
}