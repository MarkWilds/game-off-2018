using System;
using System.Collections.Generic;
using game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game
{
    public class GameApplication : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;

        private Texture2D blank;
        
        [STAThread]
        static void Main()
        {
            using (var game = new GameApplication())
                game.Run();
        }

        public GameApplication()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            EntityManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blank = Content.Load<Texture2D>("blank");

            player = new Player(
                speed: .25f, 
                texture: Content.Load<Texture2D>("Sprites/Player"), 
                position: new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2)
            );

            EntityManager.AddEntity(new Enemy(
                speed: .25f,
                texture: Content.Load<Texture2D>("Sprites/Player"),
                position: new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 3)
            ));

            //TODO: remove when done testing
            player.bulletTexture = Content.Load<Texture2D>("Sprites/Bullet");

            EntityManager.AddEntity(player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();
            EntityManager.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            EntityManager.Draw(spriteBatch);
            
            spriteBatch.Draw(blank, new Rectangle(10,10,200,200), Color.Red);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
