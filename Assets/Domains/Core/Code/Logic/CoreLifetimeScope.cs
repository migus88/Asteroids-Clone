using Migs.Asteroids.Core.Logic.Services;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Core.Logic.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Migs.Asteroids.Core.Logic
{
    public class CoreLifetimeScope : LifetimeScope
    {
        [SerializeField] private ApplicationService _applicationService;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent<IApplicationService>(_applicationService);
            builder.Register<CrossDomainService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
