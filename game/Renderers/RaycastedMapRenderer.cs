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

        private struct raycastData
        {
            public float wallFraction;
            public float distance;
            public Vector2 tileCoordinates;
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
            int cellSize = 32;

            float dtp = viewport.Width / 2 * (float) Math.Tan(halfFov);

            float anglePart = FOV / viewport.Width;
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
            spriteBatch.Draw(blankTexture, new Rectangle(0, viewport.Height / 2, viewport.Width, viewport.Height), Color.ForestGreen);

            // draw all slices
            for (int x = 0; x < viewport.Width; x++)
            {
                float angle = beginAngle + x * anglePart;
                raycastData? castData = GetIntersectionData(position, angle, isTileSolid);
                if (castData == null)
                    continue;

                // fix fisheye
                float distance = castData.Value.distance * (float) Math.Cos(halfFov);

                // get projected slice height
                int sliceHeight = (int) (cellSize / distance * dtp);

                Rectangle slice = new Rectangle(x, viewport.Height / 2 - sliceHeight / 2, 1, sliceHeight);
                spriteBatch.Draw(blankTexture, slice, Color.Orange);
            }

            RenderMinimap(spriteBatch, map, wallLayer, new Vector2(position.X / cellSize, position.Y / cellSize),
                position, orientation);
        }

        private void RenderMinimap(SpriteBatch spriteBatch, Map map, TmxLayer wallLayer, Vector2 tilecoord, Vector2 pos,
            float angle)
        {
            int cellSize = 8;
            int halfCellSize = cellSize / 2;
            if (!wallLayer.Visible)
                return;

            foreach (TmxLayerTile tile in wallLayer.Tiles)
            {
                TmxTileset tileset = map.GetTilesetForTile(tile);
                if (tileset == null)
                    continue;

                spriteBatch.Draw(blankTexture, new Rectangle(cellSize + tile.X * cellSize,
                        cellSize + tile.Y * cellSize, cellSize, cellSize),
                    new Rectangle(0, 0, 1, 1), Color.White);
            }

            Rectangle needleDestination = new Rectangle((int) (cellSize + halfCellSize + pos.X / 32 * cellSize),
                (int) (cellSize + halfCellSize + pos.Y / 32 * cellSize),
                cellSize, 2);

            spriteBatch.Draw(blankTexture, needleDestination, null, Color.Red, angle, new Vector2(-1, 0),
                SpriteEffects.None, 0);

            spriteBatch.Draw(blankTexture,
                new Rectangle((int) (cellSize + tilecoord.X * cellSize), (int) (cellSize + tilecoord.Y * cellSize),
                    cellSize, cellSize), Color.Black);
        }

        private raycastData? GetIntersectionData(Vector2 position, float angle,
            Func<Vector2, bool> isSolid)
        {
            float cos = (float) Math.Cos(angle);
            float sin = (float) Math.Sin(angle);
            float maxViewDistance = 2048;
            float cellSize = 32;

            Vector2 tileCoords = new Vector2((float) Math.Floor(position.X / cellSize),
                (float) Math.Floor(position.Y / cellSize));

            float signX = Math.Sign(cos);
            float signY = Math.Sign(sin);

            // get first point delta's as start delta
            float fX = signX < 0 ? tileCoords.X * cellSize : tileCoords.X * cellSize + cellSize;
            float fY = signY < 0 ? tileCoords.Y * cellSize : tileCoords.Y * cellSize + cellSize;

            float startX = Math.Abs((fX - position.X) / cos);
            float startY = Math.Abs((fY - position.Y) / sin);

            float deltaX = Math.Abs(cellSize / cos);
            float deltaY = Math.Abs(cellSize / sin);

            raycastData castData = new raycastData();
            while (true)
            {
                float t;
                if (startX <= startY)
                {
                    startX += deltaX;
                    t = startX;
                    tileCoords.X += signX;
                }
                else
                {
                    startY += deltaY;
                    t = startY;
                    tileCoords.Y += signY;
                }

                if (t >= maxViewDistance)
                    return null;

                if (!isSolid.Invoke(tileCoords))
                    continue;

                castData.distance = t;
                castData.tileCoordinates = tileCoords;
                return castData;
            }
        }

        private bool HasProperLayers(TmxMap mapData)
        {
            return mapData.Layers["walls"] != null;
        }
    }
}