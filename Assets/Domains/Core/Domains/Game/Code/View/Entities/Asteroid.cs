using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Entities
{
    public class Asteroid : SpaceEntity, IAsteroid
    {
        public AsteroidData Data { get; private set; }

        public void Spawn(AsteroidData data, Vector3 position, Vector3 direction)
        {
            Data = data;
            
            gameObject.SetActive(true);
            transform.position = position;
            transform.localScale = new Vector3(data.Size, data.Size, data.Size);
            transform.rotation = Quaternion.LookRotation(direction);
            Rigidbody.velocity = transform.forward * data.Speed;
        }
    }
}