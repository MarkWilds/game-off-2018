using System;
using Comora;
using game.Entities;
using game.Particles;
using game.Weapons;
using game.World;
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
        public static Texture2D PistolTexture;
        public static Texture2D RifleTexture;

        private Player player;
        private Camera camera;
        private Map hubMap;
        private TiledMapRenderer mapRenderer;
        private PlayerInterface playerInterface;

        public ScreenManager ScreenManager { get; set; }

        public void Initialize(ContentManager Content)
        {
            BulletTexture = Content.Load<Texture2D>("Sprites/Bullet");
            PistolTexture = Content.Load<Texture2D>("Sprites/Pistol");
            RifleTexture = Content.Load<Texture2D>("Sprites/Rifle");
            
            //Initialize map and camera
            mapRenderer = new TiledMapRenderer();
            camera = new Camera(ScreenManager.GraphicsDevice);
            hubMap = Map.LoadTiledMap(ScreenManager.GraphicsDevice, "Content/maps/path_debug.tmx");

            //Create the player
            player = new Player(256, Content.Load<Texture2D>("Sprites/Player"), new Vector2(256, 256));

            //Initialize some systems
            ParticleSystem.Instance.Initialize(Content);
            EntityManager.Instance.Initialize(player.playerController);

            //Add new entities
            EntityManager.Instance.AddEntity(player);
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Enemy(256, Content.Load<Texture2D>("Sprites/Enemy"),
                new Vector2(512, 512), hubMap));
            EntityManager.Instance.AddEntity(new Car(300, .075f, Content.Load<Texture2D>("Sprites/Car"), 46, 24,
                new Vector2(400, 400), (float) -Math.PI / 2));
            EntityManager.Instance.AddEntity(new AmmoPack(BulletType.AssaultRifle, 30,
                Content.Load<Texture2D>("Sprites/AmmoPack"), new Vector2(200, 300)));
            EntityManager.Instance.AddEntity(new HealthPack(25, Content.Load<Texture2D>("Sprites/HealthPack"),
                new Vector2(300, 300)));

            //Create player interface
            playerInterface = new PlayerInterface(player, Content, ScreenManager.GraphicsDevice);
            player.playerController.OnControlChanged += playerInterface.ChangeInterface;
        }

        public void Update(GameTime gameTime)
        {
            hubMap.Update(gameTime);

            UpdatePlayerLookDirection();

            EntityManager.Instance.Update(gameTime);
            ParticleSystem.Instance.Update(gameTime);

            camera.Position = ((Entity)(player.playerController.ControlledEntity)).position;
            camera.Position = new Vector2((int) camera.Position.X, (int) camera.Position.Y);

            if (InputManager.IsKeyPressed(Keys.F4))
            {
                ScreenManager.PushScreen(new ShooterScreen());
            }
        }

        private void UpdatePlayerLookDirection()
        {
            Vector2 mouseWorldPosition = InputManager.MousePosition;
            mouseWorldPosition += camera.Position;
            mouseWorldPosition.X -= camera.GetBounds().Width / 2;
            mouseWorldPosition.Y -= camera.GetBounds().Height / 2;

            // does not see to work :/
            // camera.ToWorld(ref mousePosition, out mouseWorldPosition);

            var direction = mouseWorldPosition - player.position;
            player.rotation = (float) Math.Atan2(direction.Y, direction.X);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //World
            spriteBatch.Begin(camera);
            mapRenderer.Render(hubMap, spriteBatch, camera);
            EntityManager.Instance.Draw(spriteBatch, gameTime);
            ParticleSystem.Instance.Draw(spriteBatch, gameTime);
            spriteBatch.End();


            //Interface
            spriteBatch.Begin();
            playerInterface.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            //Debug
            spriteBatch.Draw(camera.Debug);
        }

        public void Dispose()
        {
        }
    }
}