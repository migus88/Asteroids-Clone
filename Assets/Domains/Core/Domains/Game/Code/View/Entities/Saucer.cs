using System;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Settings.Behaviors;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Entities
{
    public class Saucer : SpaceEntity, ISaucer
    {
        public Vector3 ProjectileSpawnPosition => _projectileParent.position;
        public SaucerBehavior Behavior { get; private set; }

        [SerializeField] private Transform _projectileParent;
        [SerializeField] private float _rotationSpeed;

        private void Update()
        {
            ViewTransform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
        }

        public void Spawn(SaucerBehavior behavior, Vector3 position, Vector3 direction)
        {
            Behavior = behavior;
            
            var cachedTransform = transform;
            
            cachedTransform.position = position;
            cachedTransform.localScale = Vector3.one * Behavior.Size;
            Rigidbody.velocity = direction * Behavior.Speed;
        }

        public void Stop()
        {
            Rigidbody.velocity = Vector3.zero;
        }
    }
}