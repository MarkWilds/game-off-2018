using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    public class PlayerController
    {
        public IControlable ControlledEntity { get; private set; }

        public PlayerController(IControlable controlledEntity)
        {
            ControlledEntity = controlledEntity;
        }

        public void Update(GameTime gameTime)
        {
            ControlledEntity.HandleInput(gameTime);
        }

        public void ChangeControl(IControlable controlledEntity)
        {
            ControlledEntity = controlledEntity;
        }
    }
}
