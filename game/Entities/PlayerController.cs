using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Entities
{
    public class PlayerController
    {
        public delegate void ControlChanged(IControllable controlable);
        public ControlChanged OnControlChanged;

        public IControllable ControlledEntity { get; private set; }

        public PlayerController(IControllable controlledEntity)
        {
            ControlledEntity = controlledEntity;
        }

        public void HandleInput(GameTime gameTime)
        {
            ControlledEntity.HandleInput(gameTime);
        }

        public void ChangeControl(IControllable controlledEntity)
        {
            ControlledEntity = controlledEntity;
            OnControlChanged?.Invoke(controlledEntity);
        }
    }
}
