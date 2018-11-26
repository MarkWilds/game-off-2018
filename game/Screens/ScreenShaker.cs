using System;
using Microsoft.Xna.Framework;

namespace game.Screens
{
    public static class StaticScreenShaker
    {
        private static ScreenShaker instance;

        public static ScreenShaker Instance => instance ?? (instance = new ScreenShaker());
    }
    
    
    public class ScreenShaker
    {
        private float radius;
        private float angle;
        private int milliseconds;

        public void Shake(int milliseconds, float radius)
        {
            if (this.radius < radius)
                this.radius = radius;

            if (this.milliseconds < milliseconds)
                this.milliseconds = milliseconds;
        }

        public void Stop()
        {
            radius = 0;
            angle = 0;
            milliseconds = 0;
        }

        public void Update(GameTime time)
        {
            if (milliseconds <= 0)
                return;

            radius -= radius / 3;
            angle += (150 + (new Random().Next(60)));
            milliseconds -= time.ElapsedGameTime.Milliseconds;
        }

        public Vector2 GetOffset()
        {
            if (milliseconds <= 0)
                return new Vector2();
            
            var offset = new Vector2((float)(Math.Sin(angle) * radius), (float)(Math.Cos(angle) * radius));
            return offset;
        }
    }
}