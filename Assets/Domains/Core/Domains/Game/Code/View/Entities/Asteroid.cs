using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Entities
{
    public class Asteroid : SpaceEntity, IAsteroid
    {
        public AsteroidData Data { get; private set; }
        public float CurrentSpeedMultiplier { get; private set; } = 1f;

        private float _rotationSpeed;
        private Vector3 _rotationDirection;

        public void Spawn(AsteroidData data, Vector3 position, Quaternion rotation, float speedMultiplier = 1)
        {
            Data = data;
            CurrentSpeedMultiplier = speedMultiplier;

            var cachedTransform = transform;
            
            cachedTransform.localScale = new Vector3(data.Size, data.Size, data.Size);
            cachedTransform.rotation = rotation;
            Rigidbody.velocity = cachedTransform.forward * (data.Speed * speedMultiplier);
            
            cachedTransform.position = position;
            
            _rotationDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            _rotationSpeed = Random.Range(Data.MinRotationSpeed, Data.MaxRotationSpeed) * speedMultiplier;
        }

        private void Update()
        {
            ViewTransform.Rotate(_rotationDirection * _rotationSpeed * Time.deltaTime);
        }
    }
}