using System;
using System.Collections.Generic;
using System.Text;
using game.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.Entities
{
    public class GasStation : Entity, IDamageable
    {
        private List<ParticleEmitter> gasLeaks;

        public GasStation(Texture2D texture, int width, int height, Vector2 position, float rotation = 0, Rectangle source = default(Rectangle)) 
            : base(texture, width, height, position, rotation, source)
        {
            gasLeaks = new List<ParticleEmitter>();
        }

        public int MaxHealth { get; private set; } = 50;
        public int Health { get; private set; } = 50;

        public void TakeDamage(int amount, Vector2 hitDirection)
        {
            Health -= amount;
            gasLeaks.Add(new ParticleEmitter(true, true, 10, position - hitDirection * Width / 4, -hitDirection, .005f, 20, 1, .4f, ParticleShape.Circle, EmitType.OverTime, Color.Brown, Color.RosyBrown));
            if (Health <= 0)
                Explode();
        }

        private void Explode()
        {
            SpawnParticles();
            foreach (var item in gasLeaks)
            {
                item.Destroy();
            }
            Destroy();
        }
        
        private void SpawnParticles()
        {
            new ParticleEmitter(true, false, 250, position, Forward, .075f, 360, .75f, .75f, ParticleShape.Circle, EmitType.Burst, Color.DarkRed, Color.Transparent);
            new ParticleEmitter(true, false, 250, position, -Forward, .075f, 360, .75f, .75f, ParticleShape.Circle, EmitType.OverTime, Color.Black, Color.Transparent);
        }
    }
}
