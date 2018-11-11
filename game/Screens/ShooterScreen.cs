using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;

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
            RenderMinimap(spriteBatch, currentMap, currentMap.Data.Layers["walls1"], 
                new Vector2(position.X / 32, position.Y / 32), angle * (float)Math.PI / 180);
            spriteBatch.End();
        }
        
        private void RenderMinimap(SpriteBatch spriteBatch, Map map, TmxLayer wallLayer, Vector2 tilecoord, float angle)
        {
            int miniCellSize = 8;
            int halfCellSize = miniCellSize / 2;
            if (!wallLayer.Visible)
                return;

            foreach (TmxLayerTile tile in wallLayer.Tiles)
            {
                TmxTileset tileset = map.GetTilesetForTile(tile);
                if (tileset == null)
                    continue;

                spriteBatch.Draw(blankTexture, new Rectangle(miniCellSize + tile.X * miniCellSize,
                        miniCellSize + tile.Y * miniCellSize, miniCellSize, miniCellSize),
                    new Rectangle(0, 0, 1, 1), Color.Black);
            }

            Rectangle needleDestination = new Rectangle(
                (int) (miniCellSize + tilecoord.X * miniCellSize),
                (int) (miniCellSize + tilecoord.Y * miniCellSize),
                miniCellSize, 2);

            spriteBatch.Draw(blankTexture, needleDestination, null, Color.Red, angle, new Vector2(0, 1),
                SpriteEffects.None, 0);

            Rectangle playerDestination = new Rectangle(
                (int) (miniCellSize + tilecoord.X * miniCellSize - 2),
                (int) (miniCellSize + tilecoord.Y * miniCellSize - 2),
                halfCellSize, halfCellSize);

            spriteBatch.Draw(blankTexture, playerDestination, Color.Black);
        }

        public void Dispose()
        {
        }
    }
}