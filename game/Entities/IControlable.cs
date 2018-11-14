using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    public interface IControlable : IDamageable
    {
        void HandleInput(GameTime gameTime);
    }
}
