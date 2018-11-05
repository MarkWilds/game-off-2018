﻿using Microsoft.Xna.Framework;
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

        public Bullet(Texture2D texture, Vector2 position, Vector2 direction, float rotation = 0) : base(texture, position, rotation)
        {
            this.direction = direction;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= lifeSpan)
            {
                Destroy();
            }

            sprite.Position += direction * (speed * gameTime.ElapsedGameTime.Milliseconds);
        }
    }
}