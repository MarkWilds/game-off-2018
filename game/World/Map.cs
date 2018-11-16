using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyT.AStar;
using TiledSharp;

namespace game
{
    public class Map
    {
        public TmxMap Data { get; private set; }
        public Dictionary<TmxTileset, Texture2D> Textures { get; private set; }

        private Grid pathFindingGrid;

        private Map(TmxMap data, Dictionary<TmxTileset, Texture2D> textures)
        {
            this.Data = data;
            Textures = textures;

            BuildPathFindingGrid();
        }

        private void BuildPathFindingGrid()
        {
            var collisionLayer = Data.Layers.FirstOrDefault(l => l.Name == "collision");
            if (collisionLayer == default(TmxLayer))
            {
                Console.WriteLine("Map loaded without collision layer.");
                return;
            }

            pathFindingGrid = new Grid(Data.Width, Data.Height);
            foreach(var tile in collisionLayer.Tiles)
            {
                if(tile.Gid != 0)
                    pathFindingGrid.BlockCell(new Position(tile.X, tile.Y));
            }
        }

        public List<Vector2> GetPath(Position from, Position to)
        {
            var result = new List<Vector2>();
            if (pathFindingGrid == null)
                return result;

            var path = pathFindingGrid.GetPath(from, to);
            foreach (var node in path)
                result.Add(new Vector2(node.X * 32 + 16, node.Y * 32 + 16));

            return result;
        }

        public Func<RayCaster.HitData, bool> GetIsTileOccupiedFunction(string layerName)
        {
            if (!Data.Layers.Contains(layerName))
                throw new ArgumentException($"{layerName} does not exist in this map");
 
            return hitData =>
            {
                Vector2 coordinates = hitData.tileCoordinates;
                if (coordinates.X < 0 || coordinates.X >= Data.Width ||
                    coordinates.Y < 0 || coordinates.Y >= Data.Height)
                    return false;

                int index = (int) (coordinates.Y * Data.Width + coordinates.X);
                TmxLayer wallLayer = Data.Layers[layerName];
                TmxLayerTile tile = wallLayer.Tiles[index];

                // if tileset is found it is solid
                return GetTilesetForTile(tile) != null;
            };
        }

        public static Map LoadTiledMap(GraphicsDevice graphicsDevice, string pathToMap)
        {
            TmxMap data = new TmxMap(pathToMap);
            Dictionary<TmxTileset, Texture2D> tilesetTextureMap = new Dictionary<TmxTileset, Texture2D>();

            string tilesheetFolder = @"Content/Tilesets";
            foreach (TmxTileset tileset in data.Tilesets)
            {
                string pathToResource = Path.Combine(tilesheetFolder, Path.GetFileName(tileset.Image.Source));
                if (!File.Exists(pathToResource))
                    continue;

                using (FileStream stream = new FileStream(pathToResource, FileMode.Open))
                {
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, stream);
                    tilesetTextureMap.Add(tileset, texture);
                }
            }

            return new Map(data, tilesetTextureMap);
        }

        public TmxTileset GetTilesetForTile(TmxLayerTile tile)
        {
            // TODO: fix tileset choosing, might have to refactor the tiledSharp lib for it
            // we now assume that tilesets are ordered in ascending order by gid
            int tilesetCount = Data.Tilesets.Count;
            for (int i = 0; i < tilesetCount; i++)
            {
                TmxTileset tileset = Data.Tilesets[tilesetCount - 1 - i];
                if (tile.Gid >= tileset.FirstGid)
                    return tileset;
            }

            return null;
        }

        public Rectangle GetSourceRectangleForTile(TmxTileset tileset, TmxLayerTile tile)
        {
            Rectangle source = new Rectangle();
            int tileWidth = tileset.TileWidth;
            int tileHeight = tileset.TileHeight;
            int tilesInHorizontalAxis = tileset.Image.Width.GetValueOrDefault() / tileWidth;

            // depending on the tile gid get the correct tile coordinates
            int tileIndex = tile.Gid - tileset.FirstGid;
            int xTilePos = tileIndex / tilesInHorizontalAxis;
            int yTilePos = tileIndex - xTilePos * tilesInHorizontalAxis;

            source.Width = tileWidth;
            source.Height = tileHeight;

            source.X = yTilePos * tileWidth;
            source.Y = xTilePos * tileHeight;

            return source;
        }
        
        public Rectangle GetDestinationRectangleForTile(TmxTileset tileset, TmxLayerTile tile)
        {
            Rectangle destination = new Rectangle();
            int tileWidth = tileset.TileWidth;
            int tileHeight = tileset.TileHeight;

            destination.Width = tileWidth;
            destination.Height = tileHeight;

            destination.X = tile.X * tileWidth;
            destination.Y = tile.Y * tileHeight;

            return destination;
        }
    }
}