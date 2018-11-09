using game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game
{
    class Bullet : Entity
    {
        // units per second travel speed
        // a tile is currently 32 units
        private float speed = 512f;
        private float lifeSpan = 2f;
        private float timer;
        private Vector2 direction;
        private int damage;

        public Bullet(int damage, Texture2D texture, Vector2 position, Vector2 direction, float rotation = 0) 
            : base(texture, position, rotation)
        {
            this.direction = direction;
            this.damage = damage;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= lifeSpan)
            {
                Destroy();
            }

            position += direction * speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            CheckCollision();
        }

        private void CheckCollision()
        {
            var damagableObjects = EntityManager.Instance.GetDamageableEntities();
            for (int i = 0; i < damagableObjects.Length; i++)
            {
                if (BoundingBox.Intersects(damagableObjects[i].BoundingBox))
                {
                    IDamageable obj = damagableObjects[i] as IDamageable;
                    obj.TakeDamage(damage);
                    Destroy();
                }
            }
        }
    }
}
