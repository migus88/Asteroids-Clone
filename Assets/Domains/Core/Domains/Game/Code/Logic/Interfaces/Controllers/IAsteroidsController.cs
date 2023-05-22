using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Controllers
{
    public interface IAsteroidsController : IController
    {
        int SpawnedAsteroids { get; }
        
        void Reset();
        void SpawnAsteroid(int level, Vector3 position, Quaternion rotation);
    }
}