using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    public interface IGameScreen : IDisposable
    {
        ScreenManager ScreenManager { get; set; }
        
        void Initialize(ContentManager contentManager);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
