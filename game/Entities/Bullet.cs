using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    class Bullet : Entity
    {
        // units per second travel speed
        // a tile is currently 32 units
        private float speed = 512f;
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

            position += direction * speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            CheckCollision();
        }

        private void CheckCollision()
        {
            var enemies = EntityManager.Instance.GetEntitiesByType(entityToHit);
            for (int i = 0; i < enemies.Length; i++)
            {
                if (BoundingBox.Intersects(enemies[i].BoundingBox))
                {
                    Destroy();
                    enemies[i].Destroy();
                }
            }
        }
    }
}
