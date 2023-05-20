
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface ISpaceEntity
    {
        Vector3 Velocity { get; }
        Vector3 Direction { get; }
        Vector3 Position { get; }
    }
}