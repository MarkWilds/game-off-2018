using game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game.GameScreens
{
    public class OverworldScreen : IGameScreen
    {
        //TODO: Remove after testing
        public static Texture2D BulletTexture;

        public ScreenManager ScreenManager { get; set; }

        public void Initialize(ContentManager Content)
        {
            BulletTexture = Content.Load<Texture2D>("Sprites/Bullet");

            EntityManager.Instance.AddEntity(
                new Player(.25f,Content.Load<Texture2D>("Sprites/Player"),new Vector2(300, 300))
            );

            EntityManager.Instance.AddEntity(
                new Enemy(.25f,Content.Load<Texture2D>("Sprites/Enemy"),new Vector2(50, 50))
            );
        }

        public void Update(GameTime gameTime)
        {
            EntityManager.Instance.Update(gameTime);

            if (InputManager.IsKeyPressed(Keys.F4))
            {
                
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            EntityManager.Instance.Draw(spriteBatch);
        }

        public void Dispose()
        {
        }
    }
}
