using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Entities
{
    public class Asteroid : SpaceEntity, IAsteroid
    {
        public void Spawn(Vector3 position, Vector3 direction, float size, float velocity)
        {
            gameObject.SetActive(true);
            transform.localScale = new Vector3(size, size, size);
            transform.rotation = Quaternion.LookRotation(direction);
            Rigidbody.velocity = transform.forward * velocity;
        }

        public UniTask AnimateDestruction()
        {
            gameObject.SetActive(false);
            
            // TODO show particles
            
            return UniTask.CompletedTask;
        }
    }
}