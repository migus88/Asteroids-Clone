
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface ISpaceEntity
    {
        event SpaceEntityCollision Collided;
        
        Vector3 Velocity { get; }
        Vector3 Direction { get; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; }
        Bounds Bounds { get; }
        
        void Explode();
    }
    
    public delegate void SpaceEntityCollision(ISpaceEntity self);
}