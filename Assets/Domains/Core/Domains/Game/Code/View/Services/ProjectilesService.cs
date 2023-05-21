using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Settings;
using Migs.Asteroids.Game.Logic.Utils;
using Migs.Asteroids.Game.View.Entities;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Migs.Asteroids.Game.View.Services
{
    public class ProjectilesService : MonoBehaviour, IProjectilesService
    {
        [SerializeField] private int _projectileLifespanInSeconds = 1;
        [SerializeField] private LayerMask _playerProjectileLayer;
        [SerializeField] private LayerMask _enemyProjectileLayer;

        private Projectile _projectilePrefab;
        private ObjectPool<Projectile> _projectilesPool;
        private ISpaceNavigationService _spaceNavigationService;
        private ProjectileSettings _projectileSettings;
        private int _playerProjectilesActive = 0;
        
        private readonly List<Projectile> _projectiles = new();
        
        private void Awake()
        {
            _projectilesPool = new ObjectPool<Projectile>(CreateProjectile, OnProjectileRetrieved, OnProjectileReleased);
        }

        private void Update()
        {
            HandleWrapping();
        }

        [Inject]
        public void Init(ISpaceNavigationService spaceNavigationService, ProjectileSettings projectileSettings)
        {
            _spaceNavigationService = spaceNavigationService;
            _projectileSettings = projectileSettings;
        }

        public async UniTask PreloadProjectiles(int amount = 0)
        {
            if (!_projectilePrefab)
            {
                _projectilePrefab = await _projectileSettings.PrefabReference.LoadAssetAsync().Task;
            }
            
            ObjectPoolUtils.PreloadObject(_projectilesPool, amount);
        }

        public IProjectile GetAvailablePlayerProjectile(int magazineSize)
        {
            if (_playerProjectilesActive >= magazineSize)
            {
                return null;
            }
            
            var projectile = _projectilesPool.Get();
            projectile.gameObject.layer = _playerProjectileLayer.ToLayer();
            _playerProjectilesActive++;
            return projectile;
        }

        public IProjectile GetAvailableEnemyProjectile()
        {
            var projectile = _projectilesPool.Get();
            projectile.gameObject.layer = _enemyProjectileLayer.ToLayer();
            return projectile;
        }

        public void ReturnPlayerProjectile(IProjectile projectile) => ReturnProjectile(projectile);
        public void ReturnEnemyProjectile(IProjectile projectile) => ReturnProjectile(projectile);

        private void ReturnProjectile(IProjectile projectile)
        {
            var projectileObj = (Projectile)projectile;

            if (projectileObj.gameObject.layer == _playerProjectileLayer.ToLayer())
            {
                _playerProjectilesActive--;
            }
            
            _projectilesPool.Release(projectileObj);
        }

        private void OnProjectileReleased(Projectile projectile)
        {
            ObjectPoolUtils.OnObjectReleased(projectile);
            projectile.Collided -= OnProjectileCollided;
        }

        private void OnProjectileRetrieved(Projectile projectile)
        {
            ObjectPoolUtils.OnObjectRetrieved(projectile);
            projectile.Collided += OnProjectileCollided;
            StartProjectileLifespan(projectile).Forget();
        }

        private Projectile CreateProjectile()
        {
            var projectile = ObjectPoolUtils.CreateObject(_projectilePrefab, _projectilesPool.CountAll);
            _projectiles.Add(projectile);
            return projectile;
        }

        private async UniTaskVoid StartProjectileLifespan(IProjectile projectile)
        {
            await UniTask.Delay(_projectileLifespanInSeconds * 1000);
            OnProjectileCollided(projectile);
        }

        private void OnProjectileCollided(ISpaceEntity self)
        {
            var projectile = (Projectile)self;
            
            if(projectile.gameObject.activeSelf)
            {
                ReturnProjectile(projectile);
            }
        }

        private void HandleWrapping()
        {
            foreach (var projectile in _projectiles)
            {
                if (!projectile || !projectile.gameObject.activeSelf)
                {
                    continue;
                }

                _spaceNavigationService.WrapAroundGameArea(projectile);
            }
        }
    }
}