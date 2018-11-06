using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    class Bullet : Entity
    {
        private float speed = 0.5f;
        private float lifeSpan = 2f;
        private float timer;
        private Vector2 direction;
        private EntityType entityToHit;

        public Bullet(Texture2D texture, Vector2 position, Vector2 direction, EntityType entityToHit, float rotation = 0) 
            : base(texture, position, rotation)
        {
            this.direction = direction;
            this.EntityType = EntityType.Bullet;
            this.entityToHit = entityToHit;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= lifeSpan)
            {
                Destroy();
            }

            sprite.Position += direction * (speed * gameTime.ElapsedGameTime.Milliseconds);
            CheckCollision();
        }

        private void CheckCollision()
        {
            var enemies = EntityManager.Instance.GetEntitiesByType(entityToHit);
            for (int i = 0; i < enemies.Length; i++)
            {
                if (sprite.BoundingBox.Intersects(enemies[i].BoundingBox))
                {
                    Destroy();
                    enemies[i].Destroy();
                }
            }
        }
    }
}
