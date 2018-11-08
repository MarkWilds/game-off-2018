using game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class AssaultRifle : Entity, IWeapon
    {
        private float secondBetweenShots = .1f;
        private Texture2D bulletTexture;
        public Entity Owner { get; set; }

        private float timer;

        public AssaultRifle(Texture2D bulletTexture, Texture2D texture, Vector2 position, float rotation = 0)
            : base(texture, position, rotation)
        {
            this.bulletTexture = bulletTexture;
        }

        public void Shoot()
        {
            if (timer < secondBetweenShots)
                return;

            EntityManager.Instance.AddEntity(new Bullet(bulletTexture, position + Forward, Forward, EntityType.Enemy, Owner.rotation));
            timer = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position = Owner.position + (Owner.Forward * (Height /2));
            rotation = Owner.rotation;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
