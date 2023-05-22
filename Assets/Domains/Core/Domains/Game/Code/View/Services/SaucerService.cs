using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Utils;
using Migs.Asteroids.Game.View.Entities;
using UnityEngine;
using UnityEngine.Pool;

namespace Migs.Asteroids.Game.View.Services
{
    public class SaucerService : MonoBehaviour, ISaucerService
    {
        [SerializeField] private ComponentReference<Saucer> _saucerPrefabReference;
        
        private Saucer _saucerPrefab;
        private ObjectPool<Saucer> _saucers;

        private void Awake()
        {
            _saucers = new ObjectPool<Saucer>(CreateSaucer, OnSaucerRetrieved, OnSaucerReleased);
        }

        public async UniTask Preload(int amount = 0)
        {
            if (!_saucerPrefab)
            {
                _saucerPrefab = await _saucerPrefabReference.LoadAssetAsync().Task;
            }
            
            ObjectPoolUtils.PreloadObject(_saucers, amount);
        }

        public ISaucer GetObject() => _saucers.Get();

        public void ReturnObject(ISaucer saucer) => _saucers.Release((Saucer)saucer);


        private void OnSaucerReleased(Saucer obj) => ObjectPoolUtils.OnObjectReleased(obj);

        private void OnSaucerRetrieved(Saucer obj) => ObjectPoolUtils.OnObjectRetrieved(obj);

        private Saucer CreateSaucer() => ObjectPoolUtils.CreateObject(_saucerPrefab, _saucers.CountAll);
    }
}