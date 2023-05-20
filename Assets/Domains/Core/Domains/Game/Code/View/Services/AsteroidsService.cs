using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.View.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace Migs.Asteroids.Game.View.Services
{
    public class AsteroidsService : MonoBehaviour, IAsteroidsService
    {
        [SerializeField] private ComponentReference<Asteroid> _asteroidPrefabReference;

        private Asteroid _asteroidPrefab;
        private ObjectPool<Asteroid> _asteroids;

        private void Awake()
        {
            _asteroids = new ObjectPool<Asteroid>(CreateAsteroid, OnAsteroidRetrieved, OnAsteroidReleased);
        }

        private void OnAsteroidReleased(Asteroid asteroid) => asteroid.gameObject.SetActive(false);

        private void OnAsteroidRetrieved(Asteroid asteroid) => asteroid.gameObject.SetActive(true);

        private Asteroid CreateAsteroid()
        {
            if (!_asteroidPrefab)
            {
                throw new Exception("Asteroids not loaded");
            }
            
            var asteroid = Instantiate(_asteroidPrefab);
            asteroid.name = asteroid.name.Replace("Clone", _asteroids.CountAll.ToString());
            return asteroid;
        }

        public async UniTask Preload(int amount = 0)
        {
            if (!_asteroidPrefab)
            {
                _asteroidPrefab = await _asteroidPrefabReference.LoadAssetAsync().Task;
            }
            
            for (var i = 0; i < amount; i++)
            {
                var asteroid = _asteroids.Get();
                _asteroids.Release(asteroid);
            }
        }

        public IAsteroid GetAvailableAsteroid() => _asteroids.Get();

        public void ReturnAsteroid(IAsteroid asteroid) => _asteroids.Release((Asteroid)asteroid);
    }
}