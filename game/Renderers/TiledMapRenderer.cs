using System;
using System.Collections.Generic;
using Comora;
using game.Entities;
using game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace game
{
    public class TiledMapRenderer
    {
        public void Render(Map map, SpriteBatch batch, Camera camera)
        {
            foreach (TmxLayer layer in map.Data.Layers)
            {
                if (!layer.Visible)
                    continue;

                DrawVisibleTiles(batch, map, layer, camera);
            }
        }

        private void DrawVisibleTiles(SpriteBatch batch, Map map, TmxLayer layer, Camera camera)
        {
            TmxMap mapData = map.Data;
            Rectangle cameraBounds = camera.GetBounds();

            // calculate how many tiles to draw
            int cameraTilesWidth = ((int) camera.Width / mapData.TileWidth) + 2;
            int cameraTilesHeight = ((int) camera.Height / mapData.TileHeight) + 2;

            // get camera position in tiles
            int xCameraStartTile = (int) ((camera.Position.X - cameraBounds.Width / 2) / mapData.TileWidth);
            int yCameraStartTile = (int) ((camera.Position.Y - cameraBounds.Height / 2) / mapData.TileHeight);

            int xCameraEndTile = xCameraStartTile + cameraTilesWidth;
            int yCameraEndTile = yCameraStartTile + cameraTilesHeight;

            // clamp values
            ClampValue(ref xCameraStartTile, mapData.Width);
            ClampValue(ref xCameraEndTile, mapData.Width);
            ClampValue(ref yCameraStartTile, mapData.Height);
            ClampValue(ref yCameraEndTile, mapData.Height);

            for (int y = yCameraStartTile; y < yCameraEndTile; y++)
            {
                for (int x = xCameraStartTile; x < xCameraEndTile; x++)
                {
                    TmxLayerTile tile = layer.Tiles[y * mapData.Width + x];
                    DrawTile(batch, map, tile);
                }
            }
        }

        private void DrawTile(SpriteBatch batch, Map map, TmxLayerTile tile)
        {
            TmxTileset tileset = map.GetTilesetForTile(tile);
            if (tileset == null)
                return;

            Texture2D tilesetTexture = map.Textures[tileset];

            var effect = SpriteEffects.None;
            var offset = Vector2.Zero;
            var rotation = 0.0f;

            if (tile.DiagonalFlip)
            {
                rotation = -MathHelper.PiOver2;
                offset.Y -= tileset.TileHeight;

                effect |= SpriteEffects.FlipHorizontally;
            }

            if (tile.HorizontalFlip)
            {
                if (tile.DiagonalFlip)
                    effect ^= SpriteEffects.FlipVertically;
                else
                    effect ^= SpriteEffects.FlipHorizontally;
            }

            if (tile.VerticalFlip)
            {
                if (!tile.DiagonalFlip)
                    effect ^= SpriteEffects.FlipVertically;
                else
                    effect ^= SpriteEffects.FlipHorizontally;
            }

            var sourceRect = map.GetSourceRectangleForTile(tileset, tile);

            batch.Draw(
                tilesetTexture,
                new Vector2(tileset.TileWidth * tile.X, tileset.TileHeight * tile.Y) - offset,
                sourceRect,
                Color.White,
                rotation,
                Vector2.Zero,
                1,
                effect,
                0);
        }

        private void ClampValue(ref int value, int maxValue)
        {
            if (value < 0)
                value = 0;
            else if (value > maxValue)
                value = maxValue;
        }

    }
}