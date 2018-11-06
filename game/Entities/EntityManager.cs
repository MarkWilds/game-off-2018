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
        private List<Entity> entities = new List<Entity>();

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities.ToArray())
            {
                entity.Update(gameTime);
            }

            PostUpdate();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                entity.Draw(spriteBatch);
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
        /// Get all existing entities of a certain type
        /// </summary>
        /// <param name="type">The type of entities to get</param>
        /// <returns>An array of entities with the certain type</returns>
        public Entity[] GetEntitiesByType(EntityTypes type)
        {
            return entities.Where(ent => ent.EntityType == type).ToArray();
        }
    }
}
