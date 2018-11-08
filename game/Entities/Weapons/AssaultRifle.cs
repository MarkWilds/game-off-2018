using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities.Weapons
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

            EntityManager.Instance.AddEntity(new Bullet(bulletTexture, Position + Forward, Forward, EntityType.Enemy, Owner.Rotation));
            timer = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            sprite.Position = Owner.Position + (Owner.Forward * sprite.Height / 2);
            sprite.Rotation = Owner.Rotation;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
