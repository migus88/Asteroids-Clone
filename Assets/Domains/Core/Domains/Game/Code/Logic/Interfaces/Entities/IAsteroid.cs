using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface IAsteroid : ISpaceEntity
    {
        void Spawn(Vector3 position, Vector3 direction, float size, float velocity);
        UniTask AnimateDestruction();
    }
}