using game.Entities;
using game.Entities.Animations;
using game.GameScreens;
using game.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game
{
    public class Player : Entity, IControllable
    {
        private float speed;
        public WeaponManager WeaponManager { get; private set; }
        public PlayerController playerController { get; private set; }

        public int MaxHealth { get; private set; } = 100;
        public int Health { get; private set; }

        public Player(float speed, Texture2D texture, Vector2 position, float rotation = 0, Rectangle source = default(Rectangle))
            : base(texture, 32, 32, position, rotation, source)
        {
            this.speed = speed;
            Health = MaxHealth;

            animator = new Animator(32, 32);
            animator.AddAnimation(new Animation(0, 1, 0)); //Idle
            animator.AddAnimation(new Animation(1, 3, 100)); //Running

            playerController = new PlayerController(this);
            playerController.OnControlChanged += (controlledEntity) => {
                if (controlledEntity == this)
                    IsVisible = true;
                else IsVisible = false;
            };

            WeaponManager = new WeaponManager(this);
            WeaponManager.AddWeapon(new Pistol(OverworldScreen.BulletTexture, OverworldScreen.PistolTexture,
                base.position + Forward));
            WeaponManager.AddWeapon(new AssaultRifle(OverworldScreen.BulletTexture, OverworldScreen.RifleTexture,
                base.position + Forward));
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsVisible)
                return;

            base.Draw(spriteBatch, gameTime);

            WeaponManager.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            playerController.HandleInput(gameTime);

            if (!IsVisible)
                return;

            WeaponManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Handles all player input, is controlled by the playercontroller
        /// </summary>
        public void HandleInput(GameTime gameTime)
        {
            Vector2 direction = new Vector2();

            if (InputManager.IsKeyDown(Keys.A))
                direction.X -= 1;

            if (InputManager.IsKeyDown(Keys.D))
                direction.X += 1;

            if (InputManager.IsKeyDown(Keys.W))
                direction.Y -= 1;

            if (InputManager.IsKeyDown(Keys.S))
                direction.Y += 1;

            if (direction.X != 0 || direction.Y != 0)
                animator.ChangeAnimation(1);
            else
                animator.ChangeAnimation(0);

            //Normalize vector to prevent faster movement when 2 directions are pressed
            if (direction.X != 0 || direction.Y != 0)
            {
                direction.Normalize();
                position += direction * speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            //Shooting
            if (InputManager.IsMouseButtonPressed(MouseButton.Left))
                WeaponManager.ShootCurrentWeapon();

            //Weapon swap
            if (InputManager.IsKeyPressed(Keys.E) || InputManager.ScrollWheelUp)
                WeaponManager.NextWeapon();
            if (InputManager.IsKeyPressed(Keys.Q) || InputManager.ScrollWheelDown)
                WeaponManager.PreviousWeapon();
        }

        public void TakeDamage(int amount, Vector2 hitDirection)
        {
            Health -= amount;
            new ParticleEmitter(true, false, 25, position, -hitDirection, .05f, 180, .25f, 1, ParticleShape.Square, EmitType.Burst, Color.Red, Color.Red);
            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            Destroy();
        }

        public bool Heal(int amount)
        {
            if (Health == MaxHealth)
                return false;

            Health += amount;
            if (Health > MaxHealth)
                Health = MaxHealth;

            return true;
        }
    }
}