using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Core.Logic.Utils;
using UnityEngine;
using VContainer;

namespace Migs.Asteroids.Core.Views
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private SceneAssetReference[] _scenesToLoad;

        private IApplicationService _applicationService;

        [Inject]
        public void Init(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        
        private void Start()
        {
            LoadScenes().Forget();
        }

        private async UniTaskVoid LoadScenes()
        {
            foreach (var scene in _scenesToLoad)
            {
                await _applicationService.LoadDomain(scene);
            }
        }
    }
}