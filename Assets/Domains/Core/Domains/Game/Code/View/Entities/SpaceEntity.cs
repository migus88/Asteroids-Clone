using System;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Migs.Asteroids.Game.View.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class SpaceEntity : MonoBehaviour, ISpaceEntity
    {
        public event SpaceEntityCollision Collided;
        
        public Vector3 Velocity => Rigidbody.velocity;
        public float ForwardVelocity => Vector3.Dot(Velocity, Direction);
        public Vector3 Direction => ViewTransform.TransformDirection(Vector3.forward);
        public Quaternion Rotation => transform.rotation;
        public Quaternion ViewRotation => ViewTransform.rotation;
        public Bounds Bounds => ViewRenderer.bounds;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        [field:SerializeField] protected Rigidbody Rigidbody { get; set; }
        [field:SerializeField] protected Renderer ViewRenderer { get; set; }
        [field:SerializeField] protected Transform ViewTransform { get; set; }
        
        // Collision Matrix making sure that only things that should actually collide with each other are colliding
        protected virtual void OnTriggerEnter(Collider other) => Collided?.Invoke(this);

        public virtual void Explode() { }
    }
}