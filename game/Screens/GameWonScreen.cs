using game.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Screens
{
    public class GameWonScreen : IGameScreen
    {
        private float screenWidth => ScreenManager.GraphicsDevice.Viewport.Width;
        private float screenHeight => ScreenManager.GraphicsDevice.Viewport.Height;
        private Vector2 screenCenter => new Vector2(screenWidth / 2, screenHeight / 2);

        public ScreenManager ScreenManager { get; set; }
        public CursorInfo CursorInfo { get; private set; }

        private List<Button> buttons;
        private Button mainMenuButton;
        private Button exitGameButton;
        private SpriteFont arial;

        public void Initialize(ContentManager contentManager)
        {
            arial = contentManager.Load<SpriteFont>("Arial");
            CursorInfo = new CursorInfo(MouseCursor.Arrow, true);

            mainMenuButton = new Button(arial, contentManager.Load<Texture2D>("Sprites/Button"), new Vector2(screenCenter.X, screenCenter.Y - 50), "Main Menu", Color.Black);
            exitGameButton = new Button(arial, contentManager.Load<Texture2D>("Sprites/Button"), new Vector2(screenCenter.X, screenCenter.Y + 50), "Exit Game", Color.Black);

            mainMenuButton.OnClick += ReturnToMainMenu;
            exitGameButton.OnClick += ExitGame;

            buttons = new List<Button>()
            {
                mainMenuButton,
                exitGameButton
            };
        }

        private void ReturnToMainMenu(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen(new MainMenuScreen());
        }

        private void ExitGame(object sender, EventArgs e)
        {
            ScreenManager.ExitGame();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.DrawString(arial, "You Won!", new Vector2(screenCenter.X - 75, screenCenter.Y - 150), Color.Yellow);
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
