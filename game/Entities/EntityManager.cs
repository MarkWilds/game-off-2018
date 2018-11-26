using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    class EntityManager
    {
        public static EntityManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new EntityManager();
                return instance;
            }
        }
        private static EntityManager instance;
        private EntityManager() { }
        private PlayerController playerController;

        private List<Entity> entities = new List<Entity>();

        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities.ToArray())
            {
                entity.Update(gameTime);
            }

            PostUpdate();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Draw(spriteBatch, gameTime);
            }
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        /// <summary>
        /// Check if any entity has to be removed from the game
        /// </summary>
        private void PostUpdate()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].ShouldBeDestroyed)
                {
                    entities.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Get all exisiting damageable entities
        /// </summary>
        /// <returns></returns>
        public Entity[] GetEntities()
        {
            return entities.Where(entity => entity is IDamageable).ToArray();
        }

        /// <summary>
        /// Get all exisiting damageable entities in a certain range
        /// </summary>
        /// <returns></returns>
        public Entity[] GetEntitiesInRange(Vector2 position, float range)
        {
            return entities.Where(entity => entity is IDamageable && Vector2.Distance(entity.position, position) <= range).ToArray();
        }

        public IControllable GetPlayer()
        {
            return playerController.ControlledEntity;
        }

        public void ClearEntities()
        {
            foreach (var item in entities)
            {
                item.Destroy();
            }
            entities.Clear();
        }
    }
}
