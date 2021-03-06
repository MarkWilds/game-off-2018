﻿using game.Entities;
using game.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game
{
    class Bullet : Entity
    {
        // units per second travel speed
        // a tile is currently 32 units
        private float speed = 768f;
        private float lifeSpan = 2f;
        private float timer;
        private Vector2 direction;
        private int damage;

        public Bullet(int damage, Texture2D texture, Vector2 position, Vector2 direction, float rotation = 0) 
            : base(texture, 6, 6, position, rotation)
        {
            this.direction = direction;
            this.damage = damage;

            new ParticleEmitter(true, false, 5, position, -Forward * Height, .03f, 180, .25f, .25f, ParticleShape.Circle, EmitType.Burst, Color.Gray, Color.Transparent);
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= lifeSpan)
                Destroy();

            position += direction * speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            CheckCollision();
        }

        private void CheckCollision()
        {
            var damagableObjects = EntityManager.Instance.GetEntitiesInRange(position, 768);
            for (int i = 0; i < damagableObjects.Length; i++)
            {
                if (BoundingBox.Intersects(damagableObjects[i].BoundingBox))
                {
                    ((IDamageable)damagableObjects[i]).TakeDamage(damage, direction);
                    Destroy();
                }
            }
        }
    }
}
