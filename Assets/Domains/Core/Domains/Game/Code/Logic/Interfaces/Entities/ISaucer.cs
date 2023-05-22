using Migs.Asteroids.Game.Logic.Settings.Behaviors;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Entities
{
    public interface ISaucer : ISpaceEntity
    {
        Vector3 ProjectileSpawnPosition { get; }
        SaucerBehavior Behavior { get; }
        
        
        void Spawn(SaucerBehavior behavior, Vector3 position, Vector3 direction);
        void Stop();
    }
}