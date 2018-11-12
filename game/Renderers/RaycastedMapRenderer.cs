using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace game
{
    public class RaycastedMapRenderer
    {
        private readonly float FOV;
        private Texture2D blankTexture;
        private Viewport viewport;
        private int cellSize = 32;

        public RaycastedMapRenderer(Viewport view, Texture2D blank, float fov)
        {
            viewport = view;
            blankTexture = blank;
            FOV = (float) (fov * Math.PI / 180.0f);
        }

        /// <summarY>
        /// Draws the map with raycasting technique
        /// </summarY>
        /// <param name="map">The map to draw</param>
        /// <param name="position">The position to draw from</param>
        /// <param name="orientation">The rotation to draw from</param>
        public void Render(SpriteBatch spriteBatch, Map map, Vector2 position, float orientation, string wallsLayer)
        {
            TmxMap mapData = map.Data;
            if (!HasProperLayers(mapData))
                throw new Exception("This map does not contain the proper layer!");

            int slices = viewport.Width;
            float halfFov = FOV / 2;
            float distanceToProjectionPlane = slices / 2 * (float) Math.Tan(halfFov);
            
            float sliceAngle = FOV / slices;
            float beginAngle = orientation - halfFov;

            // draw ceiling and floor
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height / 2),
                Color.DarkSlateGray);
            spriteBatch.Draw(blankTexture, new Rectangle(0, viewport.Height / 2, viewport.Width, viewport.Height / 2),
                Color.DarkKhaki);

            // draw all wallslices
            for (int x = 0; x < slices; x++)
            {
                float angle = beginAngle + x * sliceAngle;

                RayCaster.HitData castData;
                if (!RayCaster.RayIntersectsGrid(position, angle, cellSize, out castData,
                    map.GetIsTileOccupiedFunction(wallsLayer)))
                    continue;

                // get the texture slice
                int tileIndex = (int) (castData.tileCoordinates.Y * mapData.Width + castData.tileCoordinates.X);
                TmxLayer wallLayer = mapData.Layers[wallsLayer];
                TmxLayerTile tile = wallLayer.Tiles[tileIndex];
                TmxTileset tileset = map.GetTilesetForTile(tile);
                if (tileset == null)
                    continue;

                // fix fisheye for distance and get slice height       
                double distance = castData.rayLength; // * Math.Cos(angle - orientation);
                int sliceHeight = (int) (cellSize * distanceToProjectionPlane / distance);

                // get drawing rectangles
                Rectangle wallRectangle = new Rectangle(x, viewport.Height / 2 - sliceHeight / 2, 1, sliceHeight);
                Rectangle textureRectangle = map.GetSourceRectangleForTile(tileset, tile);

                textureRectangle.X =
                    (int) (textureRectangle.X + (textureRectangle.Width * castData.cellFraction) % cellSize);
                textureRectangle.Width = 1;

                // get texture tint
                float dot = Vector2.Dot(castData.normal, Vector2.UnitY);
                Color lightingTint = Math.Abs(dot) > 0.9f ? Color.Gray : Color.White;

                spriteBatch.Draw(map.Textures[tileset], wallRectangle, textureRectangle, lightingTint);
            }
        }

        private bool HasProperLayers(TmxMap mapData)
        {
            return mapData.Layers["walls1"] != null;
        }
    }
}