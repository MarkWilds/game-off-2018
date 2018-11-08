using game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class Pistol : Entity, IWeapon
    {
        private float secondBetweenShots = .4f;
        private Texture2D bulletTexture;
        public Entity Owner { get; set; }

        private float timer;

        public Pistol(Texture2D bulletTexture, Texture2D texture, Vector2 position, float rotation = 0) 
            : base(texture, position, rotation)
        {
            this.bulletTexture = bulletTexture;
        }

        public void Shoot()
        {
            if (timer < secondBetweenShots)
                return;

            EntityManager.Instance.AddEntity(new Bullet(bulletTexture, position, Forward, EntityType.Enemy, Owner.rotation));
            timer = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position = Owner.position + Owner.Forward;
            rotation = Owner.rotation;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
