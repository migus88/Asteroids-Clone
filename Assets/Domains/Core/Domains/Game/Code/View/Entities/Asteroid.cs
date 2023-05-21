using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Migs.Asteroids.Game.View.Entities
{
    public class Asteroid : SpaceEntity, IAsteroid
    {
        public AsteroidData Data { get; private set; }

        private float _rotationSpeed;
        private Vector3 _rotationDirection;

        public void Spawn(AsteroidData data, Vector3 position, Quaternion rotation)
        {
            Data = data;
            
            transform.localScale = new Vector3(data.Size, data.Size, data.Size);
            transform.rotation = rotation;
            Rigidbody.velocity = transform.forward * data.Speed;
            
            transform.position = position;
            
            _rotationDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            _rotationSpeed = Random.Range(Data.MinRotationSpeed, Data.MaxRotationSpeed);
        }

        private void Update()
        {
            ViewTransform.Rotate(_rotationDirection * _rotationSpeed * Time.deltaTime);
        }
    }
}