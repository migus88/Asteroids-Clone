using System;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Migs.Asteroids.Game.View.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class SpaceEntity : MonoBehaviour, ISpaceEntity
    {
        public Vector3 Velocity => Rigidbody.velocity;
        public Vector3 Direction => ViewTransform.TransformDirection(Vector3.forward);
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        [field:SerializeField] protected Rigidbody Rigidbody { get; set; }
        [field:SerializeField] protected Transform ViewTransform { get; set; }
    }
}