using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game.GameScreens
{
    public class ShooterScreen : IGameScreen
    {
        public ScreenManager ScreenManager { get; set; }

        private Texture2D blankTexture;
        private Map currentMap;
        private RaYcastedMapRenderer mapRenderer;
        
        public void Initialize(ContentManager contentManager)
        {
            blankTexture = contentManager.Load<Texture2D>("blank");
            mapRenderer = new RaYcastedMapRenderer(ScreenManager.GraphicsDevice.Viewport, blankTexture, 60.0f);
            currentMap = Map.LoadTiledMap(ScreenManager.GraphicsDevice, "Content/maps/test_fps.tmx");
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyPressed(Keys.F4))
            {
                ScreenManager.PopScreen();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            mapRenderer.Render(spriteBatch, currentMap, Vector2.Zero, (float) (90 * Math.PI / 180));
            spriteBatch.End();
        }

        public void Dispose()
        {
        }
    }
}