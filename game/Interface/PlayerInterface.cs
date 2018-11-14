using Comora;
using game.Entities;
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
        private IDamageable controlledEntity;
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;

        //Health bar
        private Texture2D blankTexture;
        private Rectangle healthBar;
        private Rectangle healtBarBackground;
        private int healthbarHeight = 25;
        private int healthbarWidth => controlledEntity.MaxHealth;
        private int offsetX = 2;
        private int offsetY = 2;

        private int WindowHeight => graphicsDevice.Viewport.Bounds.Height;
        private int WindowWidth => graphicsDevice.Viewport.Bounds.Width;

        //Ammo
        private SpriteFont spriteFont;
        private int textHeight = 30;
        private int textWidth;

        public PlayerInterface(IControllable player, ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.controlledEntity = player;
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;

            blankTexture = contentManager.Load<Texture2D>("blank");
            spriteFont = contentManager.Load<SpriteFont>("Arial");

            healtBarBackground = new Rectangle(offsetX, WindowHeight - offsetY - healthbarHeight, healthbarWidth, healthbarHeight);
        }

        /// <summary>
        /// Change the interface to fit with the new controlledEntity
        /// </summary>
        /// <param name="newControllable"></param>
        public void ChangeInterface(IControllable newControllable)
        {
            controlledEntity = newControllable;
            healtBarBackground = new Rectangle(offsetX, WindowHeight - offsetY - healthbarHeight, healthbarWidth, healthbarHeight);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawHealthBar(spriteBatch, gameTime);

            //If the player is controller it's character we can show the ammo display
            if(controlledEntity is Player)
                DrawAmmo(spriteBatch, gameTime);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int width = (int)(healthbarWidth * ((float)controlledEntity.Health / controlledEntity.MaxHealth));
            healthBar = new Rectangle(offsetX, WindowHeight - offsetY - healthbarHeight, width, healthbarHeight);

            spriteBatch.Draw(blankTexture, healtBarBackground, Color.Black);
            spriteBatch.Draw(blankTexture, healthBar, Color.Red);
        }

        private void DrawAmmo(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Adjust the width to the amount of numbers in the text
            var weaponManager = ((Player)controlledEntity).WeaponManager;
            textWidth = weaponManager.CurrentWeaponAmmo.ToString().Length * 18;
            spriteBatch.DrawString(spriteFont, weaponManager.CurrentWeaponAmmo.ToString(), new Vector2(WindowWidth - textWidth - offsetX, WindowHeight - textHeight - offsetY), Color.Black);
        }
    }
}
