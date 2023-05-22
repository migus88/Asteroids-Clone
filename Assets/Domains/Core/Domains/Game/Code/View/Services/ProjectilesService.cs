using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
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
        private IProjectilesSettings _projectilesSettings;
        private int _playerProjectilesActive = 0;
        
        private readonly List<Projectile> _projectiles = new();
        private readonly Dictionary<IProjectile, CancellationTokenSource> _cancellationTokenSources = new();
        
        private void Awake()
        {
            _projectilesPool = new ObjectPool<Projectile>(CreateProjectile, OnProjectileRetrieved, OnProjectileReleased);
        }

        private void Update()
        {
            HandleWrapping();
        }

        [Inject]
        public void Init(ISpaceNavigationService spaceNavigationService, IProjectilesSettings projectilesSettings)
        {
            _spaceNavigationService = spaceNavigationService;
            _projectilesSettings = projectilesSettings;
        }


        public async UniTask Preload(int amount = 0)
        {
            if (!_projectilePrefab)
            {
                _projectilePrefab = await _projectilesSettings.PrefabReference.LoadAssetAsync().Task;
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

        public void ReturnPlayerProjectile(IProjectile projectile) => ReturnObject(projectile);
        public void ReturnEnemyProjectile(IProjectile projectile) => ReturnObject(projectile);

        public IProjectile GetObject() => _projectilesPool.Get();

        public void ReturnObject(IProjectile obj)
        {
            var projectileObj = (Projectile)obj;

            if (projectileObj.gameObject.layer == _playerProjectileLayer.ToLayer())
            {
                _playerProjectilesActive--;
            }
            
            _projectilesPool.Release(projectileObj);
        }

        private void OnProjectileReleased(Projectile projectile)
        {
            var hasCancellationTokenSource = _cancellationTokenSources.TryGetValue(projectile, out var tokenSource);

            if (hasCancellationTokenSource)
            {
                tokenSource.Cancel();
                _cancellationTokenSources.Remove(projectile);
            }
            
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

        private async UniTask StartProjectileLifespan(IProjectile projectile)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSources.Add(projectile, cancellationTokenSource);
            await UniTask.Delay(TimeSpan.FromSeconds(_projectileLifespanInSeconds), DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationTokenSource.Token);

            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }
            
            OnProjectileCollided(projectile);
        }

        private void OnProjectileCollided(ISpaceEntity self)
        {
            var projectile = (Projectile)self;
            
            if(projectile.gameObject.activeSelf)
            {
                ReturnObject(projectile);
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