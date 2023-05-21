
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface ISpaceEntity
    {
        event SpaceEntityCollision Collided;
        
        Vector3 Velocity { get; }
        float ForwardVelocity { get; }
        Vector3 Direction { get; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; }
        Quaternion ViewRotation { get; }
        Bounds Bounds { get; }
        
        void Explode();
    }
    
    public delegate void SpaceEntityCollision(ISpaceEntity self);
}