using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Settings;
using UnityEngine;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class PlayerController : IAsyncStartable, ITickable, IFixedTickable, IDisposable
    {
        private readonly IPlayer _player;
        private readonly IPlayerInputService _inputService;
        private readonly PlayerSettings _playerSettings;
        private readonly ProjectileSettings _projectileSettings;
        private readonly ISpaceNavigationService _spaceNavigationService;
        private readonly IProjectilesService _projectilesService;

        private bool _shouldAddForce = false;
        private float _timeSinceLastShot = 0;

        public PlayerController(IPlayer player, IPlayerInputService inputService, PlayerSettings playerSettings,
            ProjectileSettings projectileSettings, ISpaceNavigationService spaceNavigationService,
            IProjectilesService projectilesService)
        {
            _player = player;
            _inputService = inputService;
            _playerSettings = playerSettings;
            _projectileSettings = projectileSettings;
            _spaceNavigationService = spaceNavigationService;
            _projectilesService = projectilesService;

            _player.SetDrag(_playerSettings.VelocityDropRate);
            _player.Collided += OnCollision;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _projectilesService.PreloadProjectiles(_projectileSettings.PreloadAmount);
        }

        public void Tick()
        {
            HandleMovement();
            Rotate();
            Shoot();

            _spaceNavigationService.WrapAroundGameArea(_player);
        }

        public void FixedTick()
        {
            Thrust();
        }

        private void OnCollision(ISpaceEntity self)
        {
            _player.Explode();
        }

        private void Shoot()
        {
            _timeSinceLastShot += Time.deltaTime;

            if (!_inputService.IsShootingPressed || _timeSinceLastShot < _playerSettings.FireRate)
            {
                return;
            }

            var projectile = _projectilesService.GetAvailablePlayerProjectile(_playerSettings.MagazineSize);

            if (projectile == null)
            {
                return;
            }

            projectile.Spawn(_player.ProjectileSpawnPosition, _player.ViewRotation, _projectileSettings.Speed);
            _timeSinceLastShot = 0;
        }

        private void Rotate()
        {
            if (_inputService.IsRotationAxisPressed)
            {
                _player.Rotate(_inputService.RotationAxis, _playerSettings.RotationSpeed);
            }
        }

        private void HandleMovement() =>
            _shouldAddForce = _inputService.IsAccelerationPressed &&
                              _player.ForwardVelocity < _playerSettings.MaxVelocity;

        private void Thrust()
        {
            if (!_shouldAddForce)
            {
                return;
            }

            _player.AddForce(_playerSettings.ThrustersForce);
        }

        public void Dispose()
        {
            _player.Collided -= OnCollision;
        }
    }
}