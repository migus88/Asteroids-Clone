using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Entities
{
    public class Player : SpaceEntity, IPlayer
    {
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

        public void Die()
        {
            //TODO Implement
        }
    }
}