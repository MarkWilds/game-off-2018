using game.Particles;
using game.Sound;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game.Entities
{
    public class Explosion
    {
        public Explosion(Entity parent, float explosionRange, Vector2 position)
        {
            AudioManager.Instance.PlaySoundEffect("Explosion", .01f);

            new ParticleEmitter(true, false, 250, position, Vector2.UnitX, .075f, 360, .75f, .75f, ParticleShape.Circle, EmitType.Burst, Color.DarkRed, Color.Transparent);
            new ParticleEmitter(true, false, 200, position, Vector2.UnitX, .075f, 360, .75f, .75f, ParticleShape.Circle, EmitType.OverTime, Color.Black, Color.Transparent);

            var nearbyEntities = EntityManager.Instance.GetEntitiesInRange(position, explosionRange);
            foreach (IDamageable entity in nearbyEntities.ToArray())
            {
                if (entity == parent)
                    continue;

                var distance = Vector2.Distance(entity.position, position);
                var damage = (1 - (distance / explosionRange)) * 100;
                entity.TakeDamage((int)damage, (position - entity.position));
            }
        }
    }
}
