using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Entities
{
    public class Player : SpaceEntity, IPlayer
    {
        public Vector3 ProjectileSpawnPosition => _projectileParent.position;

        [SerializeField] private Transform _projectileParent;


        public void Stop()
        {
            Rigidbody.velocity = Vector3.zero;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void SetDrag(float amount)
        {
            Rigidbody.drag = amount;
        }

        public void AddForce(float force)
        {
            Rigidbody.AddForce(Direction * force, ForceMode.Force);
        }

        public void Rotate(float direction, float speed)
        {
            direction = Mathf.Clamp(direction, -1f, 1f);
            var rotationAmount = speed * direction * Time.deltaTime;
            ViewTransform.Rotate(new Vector3(0, rotationAmount, 0));
        }

        public override void Explode()
        {
            // TODO: Particles
            gameObject.SetActive(false);
        }
    }
}