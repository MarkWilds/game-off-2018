using game.Entities;
using game.Sound;
using game.Weapons;
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
        private float timer;
        public string GunShotSound { get; private set; } = "GunShot";

        public Entity Owner { get; set; }
        public BulletType BulletType => BulletType.AssaultRifle;
        public int Damage => 10;

        public AssaultRifle(Texture2D bulletTexture, Texture2D texture, Vector2 position, float rotation = 0)
            : base(texture, 32, 32, position, rotation)
        {
            this.bulletTexture = bulletTexture;
        }

        public bool Shoot()
        {
            if (timer < secondBetweenShots)
                return false;

            EntityManager.Instance.AddEntity(new Bullet(Damage, bulletTexture, position + (Forward * Height / 2), Forward, Owner.rotation));
            AudioManager.Instance.PlaySoundEffect(GunShotSound, .08f);
            timer = 0;

            return true;
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
