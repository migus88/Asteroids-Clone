using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Settings;
using UnityEngine;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class AsteroidsController : IAsyncStartable, ITickable, IFixedTickable, IDisposable
    {
        private readonly IAsteroidsService _asteroidsService;
        private readonly AsteroidSettings _asteroidSettings;
        private readonly ISpaceNavigationService _spaceNavigationService;
        
        private readonly List<IAsteroid> _asteroids = new();
        
        public AsteroidsController(IAsteroidsService asteroidsService, AsteroidSettings asteroidSettings, ISpaceNavigationService spaceNavigationService)
        {
            _asteroidsService = asteroidsService;
            _asteroidSettings = asteroidSettings;
            _spaceNavigationService = spaceNavigationService;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _asteroidsService.Preload(10);
            
            SpawnAsteroid(0, new Vector3(2,0,2), new Vector3(1f, 0, 1f));
            SpawnAsteroid(1, new Vector3(-2,0,-2), new Vector3(-1, 0, -1));
        }

        public void Tick()
        {
            _asteroids.ForEach(asteroid => _spaceNavigationService.WrapAroundGameArea(asteroid));
        }

        public void FixedTick()
        {
            
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
                var direction = updatedRotation * Vector3.forward;
                
                SpawnAsteroid(respawnData.Level, asteroid.Position, direction);
            }
        }

        private void DestroyAsteroid(IAsteroid asteroid)
        {
            asteroid.Collided -= OnAsteroidCollision;
            _asteroids.Remove(asteroid);

            asteroid.Explode();
            _asteroidsService.ReturnAsteroid(asteroid);
        }

        private void SpawnAsteroid(int level, Vector3 position, Vector3 direction)
        {
            var respawnedAsteroid = _asteroidsService.GetAvailableAsteroid();
            respawnedAsteroid.Collided += OnAsteroidCollision;
            respawnedAsteroid.Spawn(_asteroidSettings.AsteroidLevels[level], position, direction);
            _asteroids.Add(respawnedAsteroid);
        }

        public void Dispose()
        {
            _asteroids.ForEach(a => a.Collided -= OnAsteroidCollision);
        }
    }
}