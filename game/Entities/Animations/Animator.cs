using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace game.Entities.Animations
{
    public class Animator
    {
        private readonly Dictionary<int, Animation> animations;
        private Animation currentAnimation;

        private readonly int tileWidth;
        private readonly int tileHeight;

        public Animator(int tileWidth, int tileHeight)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            animations = new Dictionary<int, Animation>();
        }

        public void Update(GameTime gameTime)
        {
            currentAnimation.Update(gameTime);
        }

        public Vector2 FramePosition
            => new Vector2(currentAnimation.CurrentIndex * tileWidth, currentAnimation.Row * tileHeight);

        public void AddAnimation(Animation animation)
        {
            if (animations.ContainsKey((animation.Row)))
                throw new ArgumentException($"Animation already exists at row {animation.Row}");

            animations.Add(animation.Row, animation);

            if (currentAnimation == null)
                currentAnimation = animation;
        }

        public void ChangeAnimation(int row, bool reset = true)
        {
            if (!animations.TryGetValue(row, out var animation))
                throw new ArgumentException($"No animation registered at row {row}");

            if (row == currentAnimation.Row)
                return;

            if (reset)
                animation.Reset();

            currentAnimation = animation;
        }
    }
}