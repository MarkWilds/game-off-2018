using Microsoft.Xna.Framework;

namespace game
{
    public interface IDamageable
    {
        int Health { get; }
        int MaxHealth { get; }
        Vector2 position { get; }

        void TakeDamage(int amount, Vector2 hitDirection);
    }
}