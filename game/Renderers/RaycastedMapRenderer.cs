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
        public void Render(SpriteBatch spriteBatch, Map map, Vector2 position, float orientation)
        {
            TmxMap mapData = map.Data;
            if (!HasProperLayers(mapData))
                throw new Exception("This map does not contain the proper layer!");

            TmxLayer wallLayer = mapData.Layers["walls1"];
            float halfFov = FOV / 2;

            int slices = viewport.Width;
            float dtp = slices / 2 * (float) Math.Tan(halfFov);

            float anglePart = FOV / slices;
            float beginAngle = orientation - halfFov;

            Func<Vector2, bool> isTileSolid = coordinates =>
            {
                if (coordinates.X < 0 || coordinates.X >= mapData.Width ||
                    coordinates.Y < 0 || coordinates.Y >= mapData.Height)
                    return false;

                int index = (int) (coordinates.Y * mapData.Width + coordinates.X);
                TmxLayerTile tile = wallLayer.Tiles[index];

                // if tileset is found it is solid
                return map.GetTilesetForTile(tile) != null;
            };

            // draw ceiling and floor
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height / 2),
                Color.DarkSlateGray);
            spriteBatch.Draw(blankTexture, new Rectangle(0, viewport.Height / 2, viewport.Width, viewport.Height / 2),
                Color.DarkKhaki);

            // draw all wallslices
            for (int x = 0; x < slices; x++)
            {
                float angle = beginAngle + x * anglePart;
                
                RayCaster.RaycastData? castDataNullable =
                    RayCaster.GetIntersectionData(position, angle, cellSize, isTileSolid);
                
                if (!castDataNullable.HasValue)
                    continue;

                RayCaster.RaycastData castData = castDataNullable.Value;

                // get the texture slice
                int tileIndex = (int) (castData.tileCoordinates.Y * mapData.Width + castData.tileCoordinates.X);
                
                TmxLayerTile tile = wallLayer.Tiles[tileIndex];
                TmxTileset tileset = map.GetTilesetForTile(tile);
                if (tileset == null)
                    continue;

                Rectangle textureRectangle = new Rectangle();
                Rectangle wallRectangle = new Rectangle();
                map.GetSourceAndDestinationRectangles(tileset, tile, out textureRectangle, out wallRectangle);

                textureRectangle.X = (int) (textureRectangle.X +
                                            (textureRectangle.Width * castData.fraction) % cellSize);
                textureRectangle.Width = 1;
                
                // get distance and fix fisheye       
                double distance = castData.t * Math.Cos(orientation - angle);

                // get the size of the wall slice
                int sliceHeight = (int) (cellSize * dtp / distance);
                wallRectangle = new Rectangle(x, viewport.Height / 2 - sliceHeight / 2, 1, sliceHeight);

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