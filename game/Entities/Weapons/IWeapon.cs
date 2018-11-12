using game.Entities;
using game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game
{
    public interface IWeapon
    {
        Entity Owner { get; set; }
        BulletType BulletType { get; }
        int Damage { get; }

        bool Shoot();
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        void Update(GameTime gameTime);
    }
}
