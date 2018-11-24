using System;
using System.Collections.Generic;
using System.Text;
using game.Sound;
using game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    class AmmoPack : Entity
    {
        private BulletType bulletType;
        private int amount;

        public AmmoPack(BulletType type, int amount, Texture2D texture, Vector2 position, int width, int height, float rotation = 0, Rectangle source = default(Rectangle)) 
            : base(texture, width, height, position, rotation, source)
        {
            this.amount = amount;
            this.bulletType = type;
        }

        public override void Update(GameTime gameTime)
        {
            CheckPlayerTrigger();
        }

        private void CheckPlayerTrigger()
        {
            Player player = EntityManager.Instance.GetPlayer() as Player;

            if (player == null)
                return;

            if (player.BoundingBox.Intersects(this.BoundingBox))
            {
                player.WeaponManager.AddAmmo(bulletType, amount);
                AudioManager.Instance.PlaySoundEffect("Pickup", .1f);
                Destroy();
            }
        }
    }
}
