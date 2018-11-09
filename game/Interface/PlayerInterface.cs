using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class PlayerInterface
    {
        private WeaponManager weaponManager;
        private Player player;
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;

        //Health bar
        private Texture2D blankTexture;
        private Rectangle healthBar;
        private Rectangle healtBarBackground;
        private int offsetX = 2;
        private int height = 25;
        private int offsetY = 2;

        private int WindowHeight => graphicsDevice.Viewport.Bounds.Height;
        private int WindowWidth => graphicsDevice.Viewport.Bounds.Width;

        //Ammo
        private SpriteFont spriteFont;
        private int textHeight = 30;
        private int textWidth;

        public PlayerInterface(WeaponManager weaponManager, Player player, ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.weaponManager = weaponManager;
            this.player = player;
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;

            blankTexture = contentManager.Load<Texture2D>("blank");
            spriteFont = contentManager.Load<SpriteFont>("Arial");

            healtBarBackground = new Rectangle(offsetX, WindowHeight - offsetY - height, (int)(player.MaxHealth * 1.5f), height);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawHealthBar(spriteBatch, gameTime);
            DrawAmmo(spriteBatch, gameTime);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch, GameTime gameTime)
        {
            healthBar = new Rectangle(offsetX, WindowHeight - offsetY - height, (int)(player.Health * 1.5f), height);

            spriteBatch.Draw(blankTexture, healtBarBackground, Color.Black);
            spriteBatch.Draw(blankTexture, healthBar, Color.Red);
        }

        private void DrawAmmo(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Adjust the width to the amount of numbers in the text
            textWidth = player.WeaponManager.CurrentWeaponAmmo.ToString().Length * 18;
            spriteBatch.DrawString(spriteFont, player.WeaponManager.CurrentWeaponAmmo.ToString(), new Vector2(WindowWidth - textWidth - offsetX, WindowHeight - textHeight - offsetY), Color.Black);
        }
    }
}
