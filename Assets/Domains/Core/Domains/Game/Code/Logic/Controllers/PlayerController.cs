using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Interfaces.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Migs.Asteroids.Game.Logic.Settings;
using UnityEngine;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class PlayerController : IPlayerController, ITickable, IFixedTickable, IDisposable
    {
        private const int SecondInMillisecond = 1000;
        
        public event PlayerExplosion Exploded;
        public int Lives { get; set; }

        private readonly IPlayer _player;
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerSettings _playerSettings;
        private readonly IProjectilesSettings _projectilesSettings;
        private readonly ISpaceNavigationService _spaceNavigationService;
        private readonly IProjectilesService _projectilesService;

        private bool _shouldAddForce = false;
        private float _timeSinceLastShot = 0;
        private bool _isEnabled = true;
        private bool _canBeDestroyed = true;

        public PlayerController(IPlayer player, IPlayerInputService inputService, IPlayerSettings playerSettings,
            IProjectilesSettings projectilesSettings, ISpaceNavigationService spaceNavigationService,
            IProjectilesService projectilesService)
        {
            _player = player;
            _inputService = inputService;
            _playerSettings = playerSettings;
            _projectilesSettings = projectilesSettings;
            _spaceNavigationService = spaceNavigationService;
            _projectilesService = projectilesService;

            Reset();
            _player.Collided += OnCollision;
        }

        public async UniTask Init()
        {
            await _projectilesService.Preload(_projectilesSettings.PreloadAmount);
        }

        public void Tick()
        {
            if (!_isEnabled)
            {
                return;
            }
            
            HandleMovement();
            Rotate();
            Shoot();
            HandleHyperspace();

            _spaceNavigationService.WrapAroundGameArea(_player);
        }

        public void FixedTick()
        {
            if (!_isEnabled)
            {
                return;
            }
            
            Thrust();
        }

        private void OnCollision(ISpaceEntity self)
        {
            if (!_canBeDestroyed)
            {
                return;
            }
            
            Explode().Forget();
        }
        
        public void Reset()
        {
            Disable();
            Lives = _playerSettings.Lives;
            _player.SetDrag(_playerSettings.VelocityDropRate);
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void MakePlayerImmuneToDamage(int durationInSeconds)
        {
            MakePlayerImmuneToDamageInternal(durationInSeconds).Forget();
        }
        
        private async UniTaskVoid MakePlayerImmuneToDamageInternal(int durationInSeconds)
        {
            SetDamageImmunity(true);
            await UniTask.Delay(TimeSpan.FromSeconds(durationInSeconds));
            SetDamageImmunity(false);
        }

        private void SetDamageImmunity(bool isImmune)
        {
            _player.SetDamageImmunity(isImmune);
            _canBeDestroyed = !isImmune;
        }

        private void HandleHyperspace()
        {
            if (!_inputService.IsHyperspaceButtonPressStarted)
            {
                return;
            }

            HyperspaceJump().Forget();
        }

        private async UniTaskVoid Explode()
        {
            Disable();
            
            _player.Stop();
            _player.Explode();

            var hasMoreLives = Lives > 0;
            Exploded?.Invoke(hasMoreLives);
            
            Debug.Log($"Exploded. Has more lives? - {(hasMoreLives ? "Yes" : "No")}");

            if (!hasMoreLives)
            {
                return;
            }
            
            Lives--;

            await UniTask.Delay(_playerSettings.HyperspaceDurationInSeconds * SecondInMillisecond);
            _player.Position = _spaceNavigationService.GetCenterOfGameArea();
            
            _player.Show();
            Enable();
            
            MakePlayerImmuneToDamage(_playerSettings.ImmunityDurationOnRespawn);
        }

        private async UniTaskVoid HyperspaceJump()
        {
            Disable();
            
            _player.Stop();
            _player.Hide();
            
            _player.Position = _spaceNavigationService.GetRandomPlaceInGameArea();
            
            await UniTask.Delay(_playerSettings.HyperspaceDurationInSeconds * SecondInMillisecond);
            
            _player.Show();
            Enable();
        }

        private void Shoot()
        {
            _timeSinceLastShot += Time.deltaTime;

            if (!_inputService.IsShootingButtonPressStarted || _timeSinceLastShot < _playerSettings.FireRate)
            {
                return;
            }

            var projectile = _projectilesService.GetAvailablePlayerProjectile(_playerSettings.MagazineSize);

            if (projectile == null)
            {
                return;
            }

            projectile.Spawn(_player.ProjectileSpawnPosition, _player.ViewRotation, _projectilesSettings.Speed);
            _timeSinceLastShot = 0;
        }

        private void Rotate()
        {
            if (!_inputService.IsRotationButtonPressed)
            {
                return;
            }
            
            _player.Rotate(_inputService.RotationAxis, _playerSettings.RotationSpeed);
        }

        private void HandleMovement() =>
            _shouldAddForce = _inputService.IsAccelerationButtonPressed &&
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