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
    class Pistol : Entity, IWeapon
    {
        private Texture2D bulletTexture;
        private float timer;

        public Entity Owner { get; set; }
        public BulletType BulletType => BulletType.Pistol;
        public int Damage => 15;
        public string GunShotSound { get; private set; } = "GunShot";
        public float FireRate { get; private set; } = .4f;

        public Pistol(Texture2D bulletTexture, Texture2D texture, Vector2 position, float rotation = 0) 
            : base(texture, 16, 16, position, rotation)
        {
            this.bulletTexture = bulletTexture;
        }

        public bool Shoot()
        {
            if (timer < FireRate)
                return false;

            EntityManager.Instance.AddEntity(new Bullet(Damage, bulletTexture, position + (Forward * Height), Forward, Owner.rotation));
            AudioManager.Instance.PlaySoundEffect(GunShotSound);
            timer = 0;
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position = Owner.position + (Owner.Forward * (Height / 2));
            rotation = Owner.rotation;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
