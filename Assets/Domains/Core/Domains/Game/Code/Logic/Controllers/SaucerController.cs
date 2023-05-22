using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Interfaces.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class SaucerController : ISaucerController, ITickable, IDisposable
    {
        private readonly ISaucerService _saucerService;
        private readonly ISaucerSettings _saucerSettings;
        private readonly IPlayer _player;
        private readonly IScoreService _scoreService;
        private readonly ISpaceNavigationService _spaceNavigationService;
        private readonly IProjectilesService _projectilesService;
        private readonly IProjectilesSettings _projectilesSettings;
        private readonly ISoundService _soundService;

        private ISaucer _saucer;
        private float _timeSinceLastSaucer;
        private bool _isEnabled = false;

        public SaucerController(ISaucerService saucerService, ISaucerSettings saucerSettings, IPlayer player,
            IScoreService scoreService, ISpaceNavigationService spaceNavigationService,
            IProjectilesService projectilesService, IProjectilesSettings projectilesSettings, ISoundService soundService)
        {
            _saucerService = saucerService;
            _saucerSettings = saucerSettings;
            _player = player;
            _scoreService = scoreService;
            _spaceNavigationService = spaceNavigationService;
            _projectilesService = projectilesService;
            _projectilesSettings = projectilesSettings;
            _soundService = soundService;
        }

        public UniTask Init()
        {
            return _saucerService.Preload(_saucerSettings.MaxSaucersOnScreen);
        }

        public void Tick()
        {
            if (!_isEnabled)
            {
                return;
            }
            
            Spawn();

            if (_saucer == null)
            {
                _timeSinceLastSaucer += Time.deltaTime;
            }
            else
            {
                var isShot = _saucer.Behavior.Shoot(_saucer, _projectilesService, _projectilesSettings, _player);

                if (isShot)
                {
                    _soundService.PlaySaucerLaser();
                }

                HandleOutOfScreen();
            }
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void Reset()
        {
            Disable();
            ReleaseSaucer();
        }

        private void HandleOutOfScreen()
        {
            const float errorMargin = 0.1f;
            var isVisible = _spaceNavigationService.IsObjectOutOfGameArea(_saucer, errorMargin);

            if (!isVisible)
            {
                ReleaseSaucer();
            }
        }

        private void Spawn()
        {
            if (_saucer != null
                || _scoreService.CurrentScore < _saucerSettings.MinScoreToAppear
                || _timeSinceLastSaucer < _saucerSettings.CooldownTime)
            {
                return;
            }

            var chanceToSpawn = Random.Range(0f, 1f);

            if (chanceToSpawn > _saucerSettings.AppearanceChance)
            {
                return;
            }

            _saucer = _saucerService.GetObject();
            _saucer.Collided += OnSaucerCollision;

            var behaviorIndex = Random.Range(0, _saucerSettings.SaucerBehaviors.Length);
            var behavior = _saucerSettings.SaucerBehaviors[behaviorIndex];
            var (position, direction) = _spaceNavigationService.GetRandomSaucerSpawnPosition(_saucer.Bounds.size.y);

            _saucer.Spawn(behavior, position, direction);
            _soundService.PlaySaucerThrusters();
            
            _saucer.Behavior.Reset();
            _timeSinceLastSaucer = 0;
        }

        private void OnSaucerCollision(ISpaceEntity saucer) => DestroySaucer((ISaucer)saucer);

        private async void DestroySaucer(ISaucer saucer)
        {
            _soundService.PlaySpaceshipExplosion();
            await saucer.Explode();
            ReleaseSaucer();
            _scoreService.AddScore(saucer.Behavior.Points);
        }

        private void ReleaseSaucer()
        {
            _soundService.StopSaucerThrusters();
            
            if (_saucer == null)
            {
                return;
            }

            _saucer.Collided -= OnSaucerCollision;
            _saucerService?.ReturnObject(_saucer);
            _saucer = null;
        }

        public void Dispose()
        {
            ReleaseSaucer();
        }
    }
}