using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cathei.LinqGen;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Migs.Asteroids.Game.Logic.Settings;
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
        
        private readonly List<IAsteroid> _asteroids = new();
        
        public AsteroidsController(IAsteroidsService asteroidsService, IAsteroidsSettings asteroidsSettings, ISpaceNavigationService spaceNavigationService)
        {
            _asteroidsService = asteroidsService;
            _asteroidsSettings = asteroidsSettings;
            _spaceNavigationService = spaceNavigationService;
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

        public void SpawnAsteroid(int level, Vector3 position, Quaternion rotation)
        {
            var respawnedAsteroid = _asteroidsService.GetAvailableAsteroid();
            respawnedAsteroid.Collided += OnAsteroidCollision;
            respawnedAsteroid.Spawn(_asteroidsSettings.AsteroidLevels[level], position, rotation);
            _asteroids.Add(respawnedAsteroid);
        }

        private void OnAsteroidCollision(ISpaceEntity self)
        {
            var asteroid = (IAsteroid)self;
            DestroyAsteroid(asteroid);

            if (asteroid.Data.RespawnedAsteroidsData == null || asteroid.Data.RespawnedAsteroidsData.Length == 0)
            {
                return;
            }

            foreach (var respawnData in asteroid.Data.RespawnedAsteroidsData)
            {
                var updatedRotation = asteroid.Rotation * Quaternion.Euler(0, respawnData.RotationAngle, 0);
                
                SpawnAsteroid(respawnData.Level, asteroid.Position, updatedRotation);
            }
        }

        private void DestroyAsteroid(IAsteroid asteroid)
        {
            asteroid.Explode();
            ReleaseAsteroid(asteroid);
        }

        private void ReleaseAsteroid(IAsteroid asteroid)
        {
            asteroid.Collided -= OnAsteroidCollision;
            _asteroids?.Remove(asteroid);
            _asteroidsService?.ReturnAsteroid(asteroid);
        }

        public void Dispose()
        {
            _asteroids.ToList().ForEach(ReleaseAsteroid);
        }
    }
}