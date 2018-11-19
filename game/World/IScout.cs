using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.World
{
    public interface IScout
    {
        void RecievePath(List<Vector2> path);
    }
}
