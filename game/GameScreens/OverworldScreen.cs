using System;
using System.Collections.Generic;
using System.Text;
using game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game.GameScreens
{
    public class OverworldScreen : IGameScreen
    {
        //TODO: Remove after testing
        public static Texture2D BulletTexture;
        public static Texture2D PistolTexture;
        public static Texture2D RifleTexture;

        public bool IsPaused { get; private set; }

        public void Initialize(ContentManager Content)
        {
            BulletTexture = Content.Load<Texture2D>("Sprites/Bullet");
            PistolTexture = Content.Load<Texture2D>("Sprites/Pistol");
            RifleTexture = Content.Load<Texture2D>("Sprites/Rifle");

            EntityManager.Instance.AddEntity(
                new Player(.25f,Content.Load<Texture2D>("Sprites/Player"),new Vector2(300, 300))
            );

            EntityManager.Instance.AddEntity(
                new Enemy(.25f,Content.Load<Texture2D>("Sprites/Enemy"),new Vector2(50, 50))
            );

            EntityManager.Instance.AddEntity(new Entity(Content.Load<Texture2D>("Sprites/Car"), new Vector2(400, 400)));
        }

        public void Update(GameTime gameTime)
        {
            EntityManager.Instance.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            EntityManager.Instance.Draw(spriteBatch, gameTime);
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
