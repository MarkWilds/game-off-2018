using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class ScreenManager
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

            activeGameScreens.Add(screen);

            screen.Initialize(contentManager);
        }


        private void RemoveAllScreens()
        {
            while (!IsScreenListEmpty)
                RemoveCurrentScreen();
        }

        private void RemoveCurrentScreen()
        {
            var screen = CurrentScreen;
            screen.Dispose();
            activeGameScreens.Remove(screen);
        }

        public void PushScreen(IGameScreen screen)
        {
            if (!IsScreenListEmpty)
                CurrentScreen.Pause();

            activeGameScreens.Add(screen);

            screen.Initialize(contentManager);
        }

        public void PopScreen()
        {
            if (!IsScreenListEmpty)
                RemoveCurrentScreen();

            if (!IsScreenListEmpty)
                CurrentScreen.Resume();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsScreenListEmpty)
                if(!CurrentScreen.IsPaused)
                    CurrentScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (!IsScreenListEmpty)
                if (!CurrentScreen.IsPaused)
                    CurrentScreen.Draw(spriteBatch, gameTime);
        }
    }
}
