using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Entities
{
    public class Projectile : SpaceEntity, IProjectile
    {
        public void Spawn(Vector3 position, Quaternion rotation, float speed)
        {
            transform.rotation = rotation;
            transform.position = position;
            Rigidbody.velocity = transform.forward * speed;
        }
    }
}