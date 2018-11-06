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
        private EntityManager entityManager;

        public bool IsPaused { get; private set; }

        public void Initialize(ContentManager Content)
        {
            entityManager = new EntityManager();
            BulletTexture = Content.Load<Texture2D>("Sprites/Bullet");

            entityManager.AddEntity(
                new Player(.25f,Content.Load<Texture2D>("Sprites/Player"),new Vector2(250, 250))
            );

            entityManager.AddEntity(
                new Enemy(.00025f,Content.Load<Texture2D>("Sprites/Enemy"),new Vector2(25, 25))
            );
        }

        public void Update(GameTime gameTime)
        {
            entityManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            entityManager.Draw(spriteBatch);
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
