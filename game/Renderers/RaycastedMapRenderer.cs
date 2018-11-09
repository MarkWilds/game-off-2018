using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace game
{
    public class RaYcastedMapRenderer
    {
        private readonly float FOV;
        private Texture2D blankTeXture;
        private Viewport viewport;

        private struct raycastData
        {
            public float wallFraction;
            public Vector2 hitPoint;
            public Vector2 tileCoordinates;
        }

        public RaYcastedMapRenderer(Viewport view, Texture2D blank, float fov)
        {
            viewport = view;
            blankTeXture = blank;
            FOV = (float) (fov * Math.PI * 180.0f);
        }

        /// <summarY>
        /// Draws the map with raycasting technique
        /// </summarY>
        /// <param name="map">The map to draw</param>
        /// <param name="position">The position to draw from</param>
        /// <param name="orientation">The rotation to draw from</param>
        public void Render(SpriteBatch spriteBatch, Map map, Vector2 position, float orientation)
        {
            TmxMap mapData = map.Data;
            TmxLayer wallLayer = mapData.Layers["walls"];
            if (wallLayer == null)
                throw new Exception("This map does not contain a walls layer!");

            float fovPart = FOV / viewport.Width;
            float beginAngle = normalizeRotation(orientation + FOV / 2);

            // draw all slices
            for (int x = 0; x < viewport.Width; x++)
            {
                float angle = beginAngle - x * fovPart;
                raycastData castData = new raycastData();
                if (RayIntersectsSolid(mapData, position, angle, ref castData))
                {
                    float distance = (castData.hitPoint - position).Length();
                }
            }
        }
        
        private float normalizeRotation(float rot)
        {
            double twoPi = Math.PI * 2;
            return (float) (rot % twoPi + (rot < 0 ? twoPi : 0));
        }

        private bool RayIntersectsSolid(TmxMap mapData, Vector2 position, float angle, ref raycastData castData)
        {
            
            return false;
        }
    }
}