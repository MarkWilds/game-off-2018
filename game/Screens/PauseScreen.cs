using System;
using System.Collections.Generic;
using System.Text;
using game.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game.Screens
{
    public class PauseScreen : IGameScreen
    {
        public ScreenManager ScreenManager { get; set; }
        public CursorInfo CursorInfo { get; private set; }

        private float screenWidth => ScreenManager.GraphicsDevice.Viewport.Width;
        private float screenHeight => ScreenManager.GraphicsDevice.Viewport.Height;
        private Vector2 screenCenter => new Vector2(screenWidth / 2, screenHeight / 2);

        private List<Button> buttons;
        private Button exitGame;
        private Button resumeGame;

        private Texture2D backgroundTexture;
        private int backgroundWidth => (int)screenWidth / 3;
        private int backgroundHeight => (int)screenHeight / 2;

        public void Initialize(ContentManager contentManager)
        {
            resumeGame = new Button(contentManager.Load<SpriteFont>("Arial"), contentManager.Load<Texture2D>("Sprites/Button"), new Vector2(screenCenter.X, screenCenter.Y - 50), "Resume", Color.Black);
            exitGame = new Button(contentManager.Load<SpriteFont>("Arial"), contentManager.Load<Texture2D>("Sprites/Button"), new Vector2(screenCenter.X, screenCenter.Y + 50), "Exit", Color.Black);
            backgroundTexture = contentManager.Load<Texture2D>("blank");

            resumeGame.OnClick += ResumeGame;
            exitGame.OnClick += ExitGame;

            buttons = new List<Button>()
            {
                resumeGame,
                exitGame
            };

            CursorInfo = new CursorInfo(MouseCursor.Arrow, true);
        }

        private void ExitGame(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen(new MainMenuScreen());
        }

        private void ResumeGame(object sender, EventArgs e)
        {
            CloseMenu();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(backgroundTexture, new Rectangle((int)screenCenter.X - backgroundWidth / 2, (int)screenCenter.Y - backgroundHeight / 2, backgroundWidth, backgroundHeight), Color.Black);

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

            if (InputManager.IsKeyPressed(Keys.Escape))
                CloseMenu();
        }

        private void CloseMenu()
        {
            ScreenManager.Game.IsMouseVisible = false;
            ScreenManager.PopScreen();
        }

        public void Dispose()
        {
        }
    }
}
