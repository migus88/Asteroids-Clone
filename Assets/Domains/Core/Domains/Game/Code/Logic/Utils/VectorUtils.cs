using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Utils
{
    public static class VectorUtils
    {
        public static Vector2 ToPosition2D(this Vector3 gamePosition) => new(gamePosition.x, gamePosition.z);

        public static Vector3 ToRealPosition(this Vector2 position2D) => new(position2D.x, 0, position2D.y);
        
        public static Vector3 WrapPosition(Vector3 currentPosition, Bounds objectBounds, Vector2 bottomLeft, Vector2 topRight)
        {
            const float half = 0.5f;
            var halfWidth = objectBounds.size.x * half;
            var halfHeight = objectBounds.size.y * half;
            
            var newPosition = currentPosition.ToPosition2D();

            if (newPosition.x > topRight.x)
            {
                newPosition.x = bottomLeft.x - halfWidth;
            }
            else if (newPosition.x < bottomLeft.x)
            {
                newPosition.x = topRight.x + halfWidth;
            }
        
            if (newPosition.y > topRight.y)
            {
                newPosition.y = bottomLeft.y - halfHeight;
            }
            else if (newPosition.y < bottomLeft.y)
            {
                newPosition.y = topRight.y + halfHeight;
            }

            return newPosition.ToRealPosition();
        }
    }
}