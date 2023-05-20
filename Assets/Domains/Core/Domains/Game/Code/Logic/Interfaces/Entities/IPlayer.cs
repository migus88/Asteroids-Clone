using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface IPlayer : ISpaceEntity
    {
        void Teleport(Vector3 position);
        void SetDrag(float amount);
        void AddForce(float force);
        void Rotate(float direction, float speed);
        void Die();
    }
}