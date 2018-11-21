using System;
using System.Collections.Generic;
using System.Text;
using game.GameScreens;
using game.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game.Screens
{
    public class MainMenuScreen : IGameScreen
    {
        private float screenWidth => ScreenManager.GraphicsDevice.Viewport.Width;
        private float screenHeight => ScreenManager.GraphicsDevice.Viewport.Height;
        private Vector2 screenCenter => new Vector2(screenWidth / 2, screenHeight / 2);

        public ScreenManager ScreenManager { get; set; }

        private List<Button> buttons;
        private Button startGameButton;
        private Button exitGameButton;

        public void Initialize(ContentManager contentManager)
        {
            ScreenManager.Game.IsMouseVisible = true;

            startGameButton = new Button(contentManager.Load<SpriteFont>("Arial"), contentManager.Load<Texture2D>("Sprites/Button"), new Vector2(screenCenter.X, screenCenter.Y - 50), "Start Game", Color.Black);
            exitGameButton = new Button(contentManager.Load<SpriteFont>("Arial"), contentManager.Load<Texture2D>("Sprites/Button"), new Vector2(screenCenter.X, screenCenter.Y + 50), "Exit Game", Color.Black);

            startGameButton.OnClick += StartGame;
            exitGameButton.OnClick += ExitGame;

            buttons = new List<Button>()
            {
                startGameButton,
                exitGameButton
            };
        }

        private void StartGame(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen(new OverworldScreen());
        }

        private void ExitGame(object sender, EventArgs e)
        {
            ScreenManager.ExitGame();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (Button button in buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Button button in buttons.ToArray())
            {
                button.Update(gameTime);
            }
        }

        public void Dispose()
        {
        }
    }
}
