using System;
using System.Collections.Generic;
using System.Text;
using game.GameScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    public class DungeonEntrance : Entity
    {
        private ScreenManager screenManager;
        private Player player;

        public DungeonEntrance(ScreenManager screenManager, Texture2D texture, int width, int height, Vector2 position, float rotation = 0, Rectangle source = default(Rectangle)) 
            : base(texture, width, height, position, rotation, source)
        {
            this.screenManager = screenManager;
            player = EntityManager.Instance.GetPlayer() as Player;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (BoundingBox.Intersects(player.BoundingBox)) {
                screenManager.PushScreen(new ShooterScreen());
            }
        }
    }
}
