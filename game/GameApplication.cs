using System;
using game.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game
{
    public class GameApplication : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenManager screenManager;

        [STAThread]
        static void Main()
        {
            using (var game = new GameApplication())
                game.Run();
        }

        public GameApplication()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 640, PreferredBackBufferHeight = 400
            };
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenManager = new ScreenManager(spriteBatch, Content, GraphicsDevice, this);

            screenManager.ChangeScreen(new MainMenuScreen());
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            screenManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(gameTime);
        }
    }
}