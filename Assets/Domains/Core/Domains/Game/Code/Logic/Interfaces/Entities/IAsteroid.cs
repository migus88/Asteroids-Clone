using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Data;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface IAsteroid : ISpaceEntity
    {
        AsteroidData Data { get; }
        
        void Spawn(AsteroidData data, Vector3 position, Vector3 direction);
    }
}