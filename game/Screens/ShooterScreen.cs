using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game.GameScreens
{
    public class ShooterScreen : IGameScreen
    {
        public ScreenManager ScreenManager { get; set; }

        private Camera camera;
        private Map hubMap;
        private TiledMapRenderer mapRenderer;

        public void Initialize(ContentManager contentManager)
        {
            mapRenderer = new TiledMapRenderer();
            camera = new Camera(ScreenManager.GraphicsDevice);
            hubMap = Map.LoadTiledMap(ScreenManager.GraphicsDevice, "Content/maps/hub.tmx");
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
            spriteBatch.Begin(camera);

            mapRenderer.Render(hubMap, spriteBatch, camera);

            spriteBatch.End();
        }

        public void Dispose()
        {
        }
    }
}