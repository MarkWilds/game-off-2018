using System;
using Microsoft.Xna.Framework;

namespace game.Entities.Animations
{
    public class Animation
    {
        private double timeCounter;
        
        // Sprite sheet row index
        public int Row { get; private set; }
        
        // Amount of frames of the animation
        public int Length { get; private set; }
        
        // Milliseconds per frame
        public float Speed { get; private set; }
        
        public int CurrentIndex { get; private set; }

        public Animation(int row, int length, float speed)
        {
            Row = row;
            Length = length;
            Speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            if ((int) Speed == 0)
                return;
            
            timeCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

            while (timeCounter > Speed)
            {
                CurrentIndex++;
                
                if (CurrentIndex >= Length)
                    CurrentIndex = 0;
                
                timeCounter -= Speed;
            }
        }

        public void Reset()
        {
            CurrentIndex = 0;
            timeCounter = 0;
        }
    }
}