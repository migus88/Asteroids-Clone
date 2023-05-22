using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Interfaces.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using UnityEngine;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class AsteroidsController : IAsteroidsController, ITickable, IDisposable
    {
        public int SpawnedAsteroids => _asteroids.Count;

        private readonly IAsteroidsService _asteroidsService;
        private readonly IAsteroidsSettings _asteroidsSettings;
        private readonly ISpaceNavigationService _spaceNavigationService;
        private readonly IScoreService _scoreService;
        private readonly ISoundService _soundService;

        private readonly List<IAsteroid> _asteroids = new();

        public AsteroidsController(IAsteroidsService asteroidsService, IAsteroidsSettings asteroidsSettings,
            ISpaceNavigationService spaceNavigationService, IScoreService scoreService, ISoundService soundService)
        {
            _asteroidsService = asteroidsService;
            _asteroidsSettings = asteroidsSettings;
            _spaceNavigationService = spaceNavigationService;
            _scoreService = scoreService;
            _soundService = soundService;
        }

        public async UniTask Init()
        {
            await _asteroidsService.Preload(10);
        }

        public void Tick()
        {
            _asteroids.ForEach(asteroid => _spaceNavigationService.WrapAroundGameArea(asteroid));
        }

        public void Reset()
        {
            var asteroidsToRelease = _asteroids.ToList(); // Cloning the list in order to not modify it during iteration
            foreach (var asteroid in asteroidsToRelease)
            {
                ReleaseAsteroid(asteroid);
            }
        }

        public void SpawnAsteroid(int level, Vector3 position, Quaternion rotation, float speedMultiplier = 1)
        {
            var respawnedAsteroid = _asteroidsService.GetObject();
            respawnedAsteroid.Collided += OnAsteroidCollision;
            respawnedAsteroid.Spawn(_asteroidsSettings.AsteroidLevels[level], position, rotation, speedMultiplier);
            _asteroids.Add(respawnedAsteroid);
        }

        private async void OnAsteroidCollision(ISpaceEntity self)
        {
            var asteroid = (IAsteroid)self;
            DestroyAsteroid(asteroid).Forget();

            if (asteroid.Data.RespawnedAsteroidsData == null || asteroid.Data.RespawnedAsteroidsData.Length == 0)
            {
                return;
            }

            foreach (var respawnData in asteroid.Data.RespawnedAsteroidsData)
            {
                var updatedRotation = asteroid.Rotation * Quaternion.Euler(0, respawnData.RotationAngle, 0);

                SpawnAsteroid(respawnData.Level, asteroid.Position, updatedRotation, asteroid.CurrentSpeedMultiplier);
            }
        }

        private async UniTask DestroyAsteroid(IAsteroid asteroid)
        {
            _soundService.PlayAsteroidExplosion();
            _scoreService.AddScore(asteroid.Data.Points);
            
            await asteroid.Explode();
            ReleaseAsteroid(asteroid);
        }

        private void ReleaseAsteroid(IAsteroid asteroid)
        {
            asteroid.Collided -= OnAsteroidCollision;
            _asteroids?.Remove(asteroid);
            _asteroidsService?.ReturnObject(asteroid);
        }

        public void Dispose()
        {
            _asteroids.ToList().ForEach(ReleaseAsteroid);
        }
    }
}