using Migs.Asteroids.Core.Logic;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Input;
using Migs.Asteroids.Game.Logic.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Migs.Asteroids.Game
{
    public class GameLifetimeScope : LifetimeScope
    {
        private ICrossDomainServiceRegistrar _registrar;
        
        protected override void Configure(IContainerBuilder builder)
        {
            _registrar = Parent.Container.Resolve<ICrossDomainServiceRegistrar>();
            _registrar.RegisterService<IPlayerInputService, PlayerInputService>(builder, this);

            builder.Register<DefaultGameplayInput>(Lifetime.Singleton);
        }

        protected override void OnDestroy()
        {
            _registrar.UnRegisterService<IPlayerInputService>();
            base.OnDestroy();
        }
    }
}
