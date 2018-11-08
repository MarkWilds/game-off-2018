using System;
using System.Collections.Generic;
using System.Text;
using game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    class HealthPack : Entity
    {
        private int healAmount;

        public HealthPack(int amount, Texture2D texture, Vector2 position, float rotation = 0) : base(texture, position, rotation)
        {
            this.healAmount = amount;
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
                if (player.Heal(healAmount))
                    Destroy();
            }
        }
    }
}
