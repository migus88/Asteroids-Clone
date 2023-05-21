using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface IProjectile : ISpaceEntity
    {
        void Spawn(Vector3 position, Quaternion rotation, float speed);
    }
}