using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game
{
    public class ScreenManager
    {
        private readonly SpriteBatch spriteBatch;
        private readonly ContentManager contentManager;
        private List<IGameScreen> activeGameScreens = new List<IGameScreen>();

        public ScreenManager(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.spriteBatch = spriteBatch;
            this.contentManager = contentManager;
        }

        private IGameScreen CurrentScreen => activeGameScreens[activeGameScreens.Count - 1];
        private bool IsScreenListEmpty => activeGameScreens.Count <= 0;

        public void ChangeScreen(IGameScreen screen)
        {
            RemoveAllScreens();
            PushScreen(screen);
        }

        private void RemoveAllScreens()
        {
            while (!IsScreenListEmpty)
                RemoveCurrentScreen();
        }

        private void RemoveCurrentScreen()
        {
            CurrentScreen.Dispose();
            activeGameScreens.Remove(CurrentScreen);
        }

        public void PushScreen(IGameScreen screen)
        {
            screen.ScreenManager = this;
            activeGameScreens.Add(screen);

            screen.Initialize(contentManager);
        }

        public void PopScreen()
        {
            if (!IsScreenListEmpty)
                RemoveCurrentScreen();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsScreenListEmpty)
                CurrentScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (!IsScreenListEmpty)
                CurrentScreen.Draw(spriteBatch, gameTime);
        }
    }
}