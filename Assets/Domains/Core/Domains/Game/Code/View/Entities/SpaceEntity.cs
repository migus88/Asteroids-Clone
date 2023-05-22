using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

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
        
        [SerializeField] private ParticleSystem _explosionEffect;
        
        // Collision Matrix making sure that only things that should actually collide with each other are colliding
        protected virtual void OnTriggerEnter(Collider other) => Collided?.Invoke(this);

        public virtual async UniTask Explode()
        {
            if (!_explosionEffect)
            {
                gameObject.SetActive(false);
                return;
            }
            
            ViewTransform.gameObject.SetActive(false);
            Rigidbody.detectCollisions = false;
            
            _explosionEffect.gameObject.SetActive(true);
            _explosionEffect.Play(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_explosionEffect.main.duration));
            _explosionEffect.Stop();
            _explosionEffect.gameObject.SetActive(false);

            Rigidbody.detectCollisions = true;
            ViewTransform.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}