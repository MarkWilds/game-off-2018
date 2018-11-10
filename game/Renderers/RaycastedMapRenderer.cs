using System;
using System.Numerics;
using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace game
{
    public class RaycastedMapRenderer
    {
        private readonly float FOV;
        private Texture2D blankTexture;
        private Viewport viewport;
        private int cellSize = 32;

        private struct raycastData
        {
            public Vector2 normal;
            public float distance;
        }

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

            TmxLayer wallLayer = mapData.Layers["walls"];
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

            // draw fake floor for now
            spriteBatch.Draw(blankTexture, new Rectangle(0, viewport.Height / 2, viewport.Width, viewport.Height),
                Color.ForestGreen);

            // draw all slices
            for (int x = 0; x < slices; x++)
            {
                float angle = beginAngle + x * anglePart;
                raycastData? castData = GetIntersectionData(position, angle, isTileSolid);
                if (castData == null)
                    continue;

                // get distance and fix fisheye       
                float distance = castData.Value.distance * (float) Math.Cos(orientation - angle);

                // get projected slice height
                int sliceHeight = (int) (cellSize * dtp / distance);
                Rectangle slice = new Rectangle(x, viewport.Height / 2 - sliceHeight / 2, 1, sliceHeight);

                float dot = Vector2.Dot(castData.Value.normal, Vector2.UnitY);
                bool darken = Math.Abs(dot) > 0.9f;
                spriteBatch.Draw(blankTexture, slice, darken ? Color.Gray : Color.White);
            }

            RenderMinimap(spriteBatch, map, wallLayer, new Vector2(position.X / cellSize, position.Y / cellSize),
                orientation);
        }

        private void RenderMinimap(SpriteBatch spriteBatch, Map map, TmxLayer wallLayer, Vector2 tilecoord, float angle)
        {
            int miniCellSize = 8;
            int halfCellSize = miniCellSize / 2;
            if (!wallLayer.Visible)
                return;

            foreach (TmxLayerTile tile in wallLayer.Tiles)
            {
                TmxTileset tileset = map.GetTilesetForTile(tile);
                if (tileset == null)
                    continue;

                spriteBatch.Draw(blankTexture, new Rectangle(miniCellSize + tile.X * miniCellSize,
                        miniCellSize + tile.Y * miniCellSize, miniCellSize, miniCellSize),
                    new Rectangle(0, 0, 1, 1), Color.Black);
            }

            Rectangle needleDestination = new Rectangle(
                (int) (miniCellSize + tilecoord.X * miniCellSize),
                (int) (miniCellSize + tilecoord.Y * miniCellSize),
                miniCellSize, 2);

            spriteBatch.Draw(blankTexture, needleDestination, null, Color.Red, angle, new Vector2(0, 1),
                SpriteEffects.None, 0);

            Rectangle playerDestination = new Rectangle(
                (int) (miniCellSize + tilecoord.X * miniCellSize - 2),
                (int) (miniCellSize + tilecoord.Y * miniCellSize - 2), 
                halfCellSize, halfCellSize);
            
            spriteBatch.Draw(blankTexture, playerDestination, Color.Black);
        }

        private raycastData? GetIntersectionData(Vector2 position, float angle,
            Func<Vector2, bool> isSolid)
        {
            float cos = (float) Math.Cos(angle);
            float sin = (float) Math.Sin(angle);
            float maxViewDistance = 2048;

            Vector2 tileCoords = new Vector2((float) Math.Floor(position.X / cellSize),
                (float) Math.Floor(position.Y / cellSize));

            float signX = Math.Sign(cos);
            float signY = Math.Sign(sin);

            // get first point delta's as start delta
            float fX = signX < 0 ? tileCoords.X * cellSize : tileCoords.X * cellSize + cellSize;
            float fY = signY < 0 ? tileCoords.Y * cellSize : tileCoords.Y * cellSize + cellSize;

            float tX = Math.Abs((fX - position.X) / cos);
            float tY = Math.Abs((fY - position.Y) / sin);

            float deltaX = Math.Abs(cellSize / cos);
            float deltaY = Math.Abs(cellSize / sin);

            raycastData collisionData = new raycastData {normal = new Vector2(), distance = 0.0f};
            while (true)
            {
                if (collisionData.distance >= maxViewDistance)
                    return null;

                if (isSolid.Invoke(tileCoords))
                    break;

                if (tX <= tY)
                {
                    tileCoords.X += signX;
                    collisionData.distance = tX;
                    collisionData.normal = Vector2.UnitX * signX;
                    tX += deltaX;
                }
                else
                {
                    tileCoords.Y += signY;
                    collisionData.distance = tY;
                    collisionData.normal = Vector2.UnitY * signY;
                    tY += deltaY;
                }
            }

            return collisionData;
        }

        private bool HasProperLayers(TmxMap mapData)
        {
            return mapData.Layers["walls"] != null;
        }
    }
}