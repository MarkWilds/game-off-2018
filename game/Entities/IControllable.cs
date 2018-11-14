using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    public interface IControllable : IDamageable
    {
        void HandleInput(GameTime gameTime);
    }
}
