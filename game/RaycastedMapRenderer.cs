using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace game
{
    public class RaycastedMapRenderer
    {
        private SpriteBatch spriteBatch;
        private Texture2D blankTexture;
        private Viewport viewport;

        public RaycastedMapRenderer(Viewport view, SpriteBatch batch, Texture2D blank)
        {
            viewport = view;
            spriteBatch = batch;
            blankTexture = blank;
        }

        /// <summary>
        /// Draws the map with raycasting technique
        /// </summary>
        /// <param name="map">The map to draw</param>
        /// <param name="position">The position to draw from</param>
        /// <param name="orientation">The rotation to draw from</param>
        public void Render(TmxMap map, Vector2 position, float orientation)
        {
            float fov = (float) (60.0 * Math.PI / 180.0);
            
            float fovPart = fov / viewport.Width;
            float beginAngle = normalizeRotation(orientation - fov / 2);
            
            // draw all slices
            for (int x = 0; x < viewport.Width; x++)
            {
                float angle = beginAngle + x * fovPart;
            }
        }

        private float normalizeRotation(float rot)
        {
            double twoPi = Math.PI * 2;
            return (float) (rot % twoPi + (rot < 0 ? twoPi : 0));
        }
    }
}