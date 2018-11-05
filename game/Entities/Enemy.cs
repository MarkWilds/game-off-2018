using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    class Enemy : Entity
    {
        public float speed;

        public Enemy(float speed, Texture2D texture, Vector2 position, float rotation = 0)
            : base(texture, position, rotation)
        {
            this.speed = speed;
            this.EntityType = EntityTypes.Enemy;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            //Move
            //Shoot
            //Check collisions
        }
    }
}
