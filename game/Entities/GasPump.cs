using System;
using System.Collections.Generic;
using System.Text;
using game.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    public class GasPump : Entity, IDamageable
    {
        private List<ParticleEmitter> gasLeaks;
        private float explosionRange = 150;

        public GasPump(Texture2D texture, int width, int height, Vector2 position, float rotation = 0, Rectangle source = default(Rectangle)) 
            : base(texture, width, height, position, rotation, source)
        {
            gasLeaks = new List<ParticleEmitter>();
        }

        public int MaxHealth { get; private set; } = 50;
        public int Health { get; private set; } = 50;

        public void TakeDamage(int amount, Vector2 hitDirection)
        {
            if (Health <= 0)
                return;

            Health -= amount;
            gasLeaks.Add(new ParticleEmitter(true, true, 10, position - hitDirection * Width / 3, -hitDirection, .005f, 20, 1, .4f, ParticleShape.Circle, EmitType.OverTime, Color.Brown, Color.RosyBrown));

            if (Health <= 0)
                Explode();
        }

        private void Explode()
        {
            foreach (var emitter in gasLeaks)
            {
                emitter.Destroy();
            }
            new Explosion(this, 100, position);
            Destroy();
        }
    }
}
