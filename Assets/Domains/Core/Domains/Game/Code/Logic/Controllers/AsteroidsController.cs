using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
            
            var asteroid = _asteroidsService.GetAvailableAsteroid();
            asteroid.Spawn(new Vector3(2,0,2), new Vector3(0f, 0, 1f), _asteroidSettings.AsteroidLevels[0].Size, _asteroidSettings.AsteroidLevels[0].Speed);
            _asteroids.Add(asteroid);
            
            
            var asteroid2 = _asteroidsService.GetAvailableAsteroid();
            asteroid2.Spawn(new Vector3(-2,0,-2), new Vector3(-1, 0, -1), _asteroidSettings.AsteroidLevels[1].Size, _asteroidSettings.AsteroidLevels[1].Speed);
            _asteroids.Add(asteroid2);

            _asteroids.ForEach(a => a.Collided += OnAsteroidCollision);
        }

        public void Tick()
        {
            _asteroids.ForEach(asteroid => _spaceNavigationService.WrapAroundGameArea(asteroid));
        }

        public void FixedTick()
        {
            
        }

        private void OnAsteroidCollision(ISpaceEntity self, ISpaceEntity other)
        {
            Debug.Log($"Boom!");
        }

        public void Dispose()
        {
            _asteroids.ForEach(a => a.Collided -= OnAsteroidCollision);
        }
    }
}