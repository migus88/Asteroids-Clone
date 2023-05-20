using Migs.Asteroids.Game.Data;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Utils
{
    public static class VectorUtils
    {
        public static Vector2 ToPosition2D(this Vector3 gamePosition) => new(gamePosition.x, gamePosition.z);

        public static Vector3 ToRealPosition(this Vector2 position2D) => new(position2D.x, 0, position2D.y);
        
        public static Vector3 WrapPosition(Vector3 currentPosition, Bounds objectBounds, Rectangle area)
        {
            const float half = 0.5f;
            var halfWidth = objectBounds.size.x * half;
            var halfHeight = objectBounds.size.y * half;
            
            var newPosition = currentPosition.ToPosition2D();

            if (newPosition.x > area.MaxX)
            {
                newPosition.x = area.MinX - halfWidth;
            }
            else if (newPosition.x < area.MinX)
            {
                newPosition.x = area.MaxX + halfWidth;
            }
        
            if (newPosition.y > area.MaxY)
            {
                newPosition.y = area.MinY - halfHeight;
            }
            else if (newPosition.y < area.MinY)
            {
                newPosition.y = area.MaxY + halfHeight;
            }

            return newPosition.ToRealPosition();
        }
    }
}