using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Utils;
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

        private void OnAsteroidReleased(Asteroid asteroid) => ObjectPoolUtils.OnObjectReleased(asteroid);

        private void OnAsteroidRetrieved(Asteroid asteroid) => ObjectPoolUtils.OnObjectRetrieved(asteroid);

        private Asteroid CreateAsteroid() => ObjectPoolUtils.CreateObject(_asteroidPrefab, _asteroids.CountAll);

        public async UniTask Preload(int amount = 0)
        {
            if (!_asteroidPrefab)
            {
                _asteroidPrefab = await _asteroidPrefabReference.LoadAssetAsync().Task;
            }
            
            ObjectPoolUtils.PreloadObject(_asteroids, amount);
        }

        public IAsteroid GetObject() => _asteroids.Get();

        public void ReturnObject(IAsteroid obj) => _asteroids.Release((Asteroid)obj);
    }
}