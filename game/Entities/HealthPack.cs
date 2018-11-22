using System;
using System.Collections.Generic;
using System.Text;
using game.Sound;
using game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    class HealthPack : Entity
    {
        private int healAmount;

        public HealthPack(int amount, Texture2D texture, Vector2 position, int width, int height, float rotation = 0, Rectangle source = default(Rectangle)) 
            : base(texture, width, height, position, rotation, source)
        {
            this.healAmount = amount;
        }

        public override void Update(GameTime gameTime)
        {
            CheckPlayerTrigger();
        }

        private void CheckPlayerTrigger()
        {
            if (!(EntityManager.Instance.GetPlayer() is Player player))
                return;

            if (player.BoundingBox.Intersects(this.BoundingBox))
            {
                if (player.Heal(healAmount))
                {
                    AudioManager.Instance.PlaySoundEffect("Pickup");
                    Destroy();
                }
            }
        }
    }
}
