using game.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    public class Collectable : Entity
    {
        public Collectable(Texture2D texture, Vector2 position, int width, int height, float rotation = 0, Rectangle source = default(Rectangle))
            : base(texture, width, height, position, rotation, source)
        {
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
                player.PickUpCollectable();
                AudioManager.Instance.PlaySoundEffect("Pickup", .1f);
                Destroy();
            }
        }
    }
}
