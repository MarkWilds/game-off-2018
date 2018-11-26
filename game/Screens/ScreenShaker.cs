using System;
using Microsoft.Xna.Framework;

namespace game.Screens
{
    public static class StaticScreenShaker
    {
        private static ScreenShaker _instance;

        public static ScreenShaker Instance => _instance ?? (_instance = new ScreenShaker());
    }
    
    
    public class ScreenShaker
    {
        private float _radius;
        private float _angle;
        private int _milliseconds;

        public void Shake(int milliseconds, float radius)
        {
            if (_radius < radius)
                _radius = radius;

            if (_milliseconds < milliseconds)
                _milliseconds = milliseconds;
        }

        public void Stop()
        {
            _radius = 0;
            _angle = 0;
            _milliseconds = 0;
        }

        public void Update(GameTime time)
        {
            if (_milliseconds <= 0)
                _radius = _radius - (time.ElapsedGameTime.Milliseconds / 100f);
            
            if (_radius <= 0)
                return;

            _angle += (150 + (new Random().Next(60)));
            _milliseconds -= time.ElapsedGameTime.Milliseconds;
        }

        public Vector2 GetOffset()
        {
            if (_radius <= 0 && _milliseconds <= 0)
                return new Vector2();
            
            var offset = new Vector2((float)(Math.Sin(_angle) * _radius), (float)(Math.Cos(_angle) * _radius));
            return offset;
        }
    }
}