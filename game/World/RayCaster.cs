using System;
using Microsoft.Xna.Framework;

namespace game
{
    public class RayCaster
    {
        private const float MaxViewDistance = 2048;

        public struct RaycastData
        {
            public Vector2 normal;
            public Vector2 tileCoordinates;
            public float fraction;
            public float t;
        }

        public static RaycastData? GetIntersectionData(Vector2 position, float angle, int cellSize,
            Func<Vector2, bool> isSolid)
        {
            float cos = (float) Math.Cos(angle);
            float sin = (float) Math.Sin(angle);

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

            RaycastData collisionData = new RaycastData {normal = new Vector2(), t = 0.0f};
            while (true)
            {
                if (collisionData.t >= MaxViewDistance)
                    return null;

                if (isSolid.Invoke(tileCoords))
                    return collisionData;

                if (tX <= tY)
                {
                    tileCoords.X += signX;
                    collisionData.t = tX;                    
                    collisionData.normal = Vector2.UnitX * signX;
                    collisionData.tileCoordinates = tileCoords;
                    collisionData.fraction = (position.Y + tX * sin) % cellSize / cellSize;
                    tX += deltaX;
                }
                else
                {
                    tileCoords.Y += signY;
                    collisionData.t = tY;                   
                    collisionData.normal = Vector2.UnitY * signY;
                    collisionData.tileCoordinates = tileCoords;
                    collisionData.fraction = (position.X + tY * cos) % cellSize / cellSize;
                    tY += deltaY;
                }
            }
        }
    }
}