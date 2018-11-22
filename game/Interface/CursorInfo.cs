using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Interface
{
    public class CursorInfo
    {
        public MouseCursor CursorTexture { get; private set; }
        public bool ShowCursor { get; private set; }

        public CursorInfo(MouseCursor cursorTexture, bool showCursor)
        {
            CursorTexture = cursorTexture;
            ShowCursor = showCursor;
        }
    }
}
