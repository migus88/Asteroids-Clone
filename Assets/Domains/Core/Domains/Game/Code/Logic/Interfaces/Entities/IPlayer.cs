using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface IPlayer : ISpaceEntity
    {
        Bounds Bounds { get; }
        
        void Teleport(Vector3 position);
        void SetDrag(float amount);
        void AddForce(float force);
        void Rotate(float direction, float speed);
        void Die();
    }
}