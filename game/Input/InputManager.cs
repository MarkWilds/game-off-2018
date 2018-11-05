using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    public static class InputManager
    {
        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;

        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        public static float MouseAxisX => currentMouseState.X - previousMouseState.X;
        public static float MouseAxisY => currentMouseState.Y - previousMouseState.Y;

        public static Vector2 MouseWorldPosition => currentMouseState.Position.ToVector2();

        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsMouseButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return currentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return currentMouseState.RightButton == ButtonState.Pressed;
                default:
                    throw new Exception("That is not a valid mouse button");
            }
        }

        public static bool MouseButtonClicked(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
                case MouseButton.Right:
                    return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
                default:
                    throw new Exception("That is not a valid mouse button");
            }
        }

        public static void Update()
        {
            
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }
    }
}
