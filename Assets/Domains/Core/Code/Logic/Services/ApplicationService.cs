using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Enums;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Core.Logic.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Migs.Asteroids.Core.Logic.Services
{
    internal class ApplicationService : MonoBehaviour, IApplicationService
    {
        public ApplicationState State { get; private set; }
        
        public event ApplicationStateChangedDelegate ApplicationStateChanged;

        private LifetimeScope _coreLifetimeScope;
        
        [Inject]
        public void Init(LifetimeScope lifetimeScope)
        {
            _coreLifetimeScope = lifetimeScope;
        }
        
        public async UniTask LoadDomain(SceneAssetReference domainReference)
        {
            using (LifetimeScope.EnqueueParent(_coreLifetimeScope))
            {
                await domainReference.LoadSceneAsync(LoadSceneMode.Additive);
            }
        }

        private void OnEnable()
        {
            State = ApplicationState.Active;
            ApplicationStateChanged?.Invoke(State);
        }

        private void OnDisable()
        {
            State = ApplicationState.Inactive;
            ApplicationStateChanged?.Invoke(State);
        }
    }

    public delegate void ApplicationStateChangedDelegate(ApplicationState state);
}