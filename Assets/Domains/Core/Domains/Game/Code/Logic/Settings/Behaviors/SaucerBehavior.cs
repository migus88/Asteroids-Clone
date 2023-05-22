using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings.Behaviors
{
    public abstract class SaucerBehavior : ScriptableObject
    {
        [field:SerializeField] public string Name { get; set; }
        [field:SerializeField] public float Size { get; set; }
        [field:SerializeField] public float Speed { get; set; }
        [field:SerializeField] public int Points { get; set; }
        [field:SerializeField] private float FireRate { get; set; }

        private float _timeSinceLastShot;

        public virtual void Reset()
        {
            _timeSinceLastShot = 0;
        }
        
        public virtual bool Shoot(ISaucer saucer, IProjectilesService projectilesService, IProjectilesSettings projectilesSettings, IPlayer player)
        {
            _timeSinceLastShot += Time.deltaTime;

            if (saucer == null || _timeSinceLastShot < FireRate)
            {
                return false;
            }

            var projectile = projectilesService.GetAvailableEnemyProjectile();

            if (projectile == null)
            {
                return false;
            }

            var rotation = GetProjectileDirection(saucer, player);

            projectile.Spawn(saucer.ProjectileSpawnPosition, rotation, projectilesSettings.Speed);
            _timeSinceLastShot = 0;

            return true;
        }

        protected abstract Quaternion GetProjectileDirection(ISaucer saucer, IPlayer player);
    }
}