using System;

namespace Migs.Asteroids.Game.Data
{
    [Serializable]
    public struct AsteroidData
    {
        public string Name;
        public int Level;
        public float Size;
        public float Speed;
        public int Points;
    }
}