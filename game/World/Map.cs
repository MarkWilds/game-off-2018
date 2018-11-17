using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using game.Entities;
using game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace game
{
    public class Map
    {
        public TmxMap Data { get; private set; }
        public Dictionary<TmxTileset, Texture2D> Textures { get; private set; }

        private Map(TmxMap data, Dictionary<TmxTileset, Texture2D> textures)
        {
            this.Data = data;
            Textures = textures;
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

        public void GetSourceAndDestinationRectangles(TmxTileset tileset, TmxLayerTile tile,
            out Rectangle source, out Rectangle destination)
        {
            int tileWidth = tileset.TileWidth;
            int tileHeight = tileset.TileHeight;
            int tilesInHorizontalAxis = tileset.Image.Width.GetValueOrDefault() / tileWidth;

            // depending on the tile gid get the correct tile coordinates
            int tileIndex = tile.Gid - tileset.FirstGid;
            int xTilePos = tileIndex / tilesInHorizontalAxis;
            int yTilePos = tileIndex - xTilePos * tilesInHorizontalAxis;

            source.Width = destination.Width = tileWidth;
            source.Height = destination.Height = tileHeight;

            source.X = yTilePos * tileWidth;
            source.Y = xTilePos * tileHeight;
            destination.X = tile.X * tileWidth;
            destination.Y = tile.Y * tileHeight;
        }

        public void LoadObjects()
        {
            foreach (var obj in Data.ObjectGroups[0].Objects)
            {
                var tile = obj.Tile;
                Rectangle source, destination;
                TmxTileset tileset = GetTilesetForTile(tile);

                if (tileset == null)
                    continue;

                Texture2D tilesetTexture = Textures[tileset];
                GetSourceAndDestinationRectangles(tileset, obj.Tile, out source, out destination);

                CreateEntity(obj, source, tileset, tilesetTexture);
            }
        }

        private static void CreateEntity(TmxObject obj, Rectangle source, TmxTileset tileset, Texture2D tilesetTexture)
        {
            var random = new Random();
            var tileId = obj.Tile.Gid - tileset.FirstGid;
            var type = tileset.Tiles[tileId].Type;

            var rotation = MathHelper.ToRadians((float)obj.Rotation);
            var spawnPosition = GetObjectPosition(obj);

            switch (type)
            {
                case "Dungeon_Entrance":
                    break;

                case "Ammo":
                    var randomBulletType = (BulletType)random.Next(Enum.GetNames(typeof(BulletType)).Length);
                    EntityManager.Instance.AddEntity(new AmmoPack(randomBulletType, random.Next(15, 30), tilesetTexture, spawnPosition, rotation, source));
                    break;
                case "Health":
                    EntityManager.Instance.AddEntity(new HealthPack(random.Next(15, 30), tilesetTexture, spawnPosition, rotation, source));
                    break;
                default:
                    EntityManager.Instance.AddEntity(new Entity(tilesetTexture, (int)obj.Width, (int)obj.Height, spawnPosition, rotation, source));
                    break;
            }
        }

        private static Vector2 GetObjectPosition(TmxObject obj)
        {
            var position = new Vector2((int)(obj.X + obj.Width / 2), (int)(obj.Y - obj.Height / 2));

            if (obj.Rotation == 90 || obj.Rotation == -270)
                position.Y += (int)obj.Height;

            else if(obj.Rotation == 180 || obj.Rotation == -180)
            {
                position.Y += (int)obj.Height;
                position.X -= (int)obj.Width;
            }

            else if(obj.Rotation == 270 || obj.Rotation == -90)
                position.X -= (int)obj.Width;

            return position;
        }
    }
}