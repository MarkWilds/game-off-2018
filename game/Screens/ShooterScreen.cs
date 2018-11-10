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
        private RaycastedMapRenderer mapRenderer;

        private Vector2 position = new Vector2(32 + 16, 64 + 16);
        private float movementSpeed = 64;
        private float angle;

        public void Initialize(ContentManager contentManager)
        {
            blankTexture = contentManager.Load<Texture2D>("blank");
            mapRenderer = new RaycastedMapRenderer(ScreenManager.GraphicsDevice.Viewport, blankTexture, 90.0f);
            currentMap = Map.LoadTiledMap(ScreenManager.GraphicsDevice, "Content/maps/test_fps.tmx");
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyPressed(Keys.F4))
            {
                ScreenManager.PopScreen();
            }

            float verticalMovement = 0.0f;
            float horizontalMovement = 0.0f;

            if (InputManager.IsKeyDown(Keys.A))
                horizontalMovement = -1.0f;
            else if (InputManager.IsKeyDown(Keys.D))
                horizontalMovement = 1.0f;

            if (InputManager.IsKeyDown(Keys.W))
                verticalMovement = 1.0f;
            else if (InputManager.IsKeyDown(Keys.S))
                verticalMovement = -1.0f;


            Vector2 forward = new Vector2((float) Math.Cos(angle * Math.PI / 180),
                (float) Math.Sin(angle * Math.PI / 180));
            Vector2 right = new Vector2(-forward.Y, forward.X);

            Vector2 movementDirection = forward * verticalMovement + right * horizontalMovement;

            if (verticalMovement != 0.0f || horizontalMovement != 0.0f)
            {
                movementDirection.Normalize();

                position += movementDirection * movementSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            angle += InputManager.MouseAxisX * 10.0f * (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            mapRenderer.Render(spriteBatch, currentMap, position, angle * (float) (Math.PI / 180));
            spriteBatch.End();
        }

        public void Dispose()
        {
        }
    }
}