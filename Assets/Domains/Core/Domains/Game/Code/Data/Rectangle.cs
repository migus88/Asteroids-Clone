using UnityEngine;

namespace Migs.Asteroids.Game.Data
{
    public struct Rectangle
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;

        public Rectangle(Vector2 bottomLeft, Vector2 topRight)
        {
            MinX = bottomLeft.x;
            MaxX = topRight.x;
            MinY = bottomLeft.y;
            MaxY = topRight.y;
        }
    }
}