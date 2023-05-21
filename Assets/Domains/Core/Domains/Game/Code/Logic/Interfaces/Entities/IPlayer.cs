using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface IPlayer : ISpaceEntity
    {
        Vector3 ProjectileSpawnPosition { get; }

        void Stop();
        void Hide();
        void Show();
        void SetDrag(float amount);
        void AddForce(float force);
        void Rotate(float direction, float speed);
    }
}