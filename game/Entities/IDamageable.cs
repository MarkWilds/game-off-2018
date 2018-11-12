using Microsoft.Xna.Framework;

namespace game
{
    public interface IDamageable
    {
        int Health { get; }

        void TakeDamage(int amount, Vector2 hitDirection);
    }
}