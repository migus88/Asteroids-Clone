using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Input;
using Migs.Asteroids.Game.Logic.Controllers;
using Migs.Asteroids.Game.Logic.Handlers;
using Migs.Asteroids.Game.Logic.Interfaces.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Managers;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Migs.Asteroids.Game.Logic.Managers;
using Migs.Asteroids.Game.Logic.Services;
using Migs.Asteroids.Game.Logic.Settings;
using Migs.Asteroids.Game.View.Entities;
using Migs.Asteroids.Game.View.Services;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Player _player;
        
        [Header("Services")]
        [SerializeField] private ViewportService _viewportService;
        [SerializeField] private AsteroidsService _asteroidsService;
        [SerializeField] private ProjectilesService _projectilesService;
        
        [Header("Settings")]
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private ProjectilesSettings _projectilesSettings;
        [SerializeField] private AsteroidsSettings _asteroidsSettings;
        [SerializeField] private GameSettings _gameSettings;

        private ICrossDomainServiceRegistrar _registrar;
        
        protected override void Configure(IContainerBuilder builder)
        {
            _registrar = Parent.Container.Resolve<ICrossDomainServiceRegistrar>();
            _registrar.RegisterService<IPlayerInputService, PlayerInputService>(builder, this);

            builder.Register<DefaultGameplayInput>(Lifetime.Singleton);
            builder.Register<ISpaceNavigationService, SpaceNavigationService>(Lifetime.Singleton);
            builder.Register<IRoundsService, RoundsService>(Lifetime.Singleton);
            
            builder.RegisterComponent<IPlayer>(_player);
            builder.RegisterComponent<IViewportService>(_viewportService);
            builder.RegisterComponent<IAsteroidsService>(_asteroidsService);
            builder.RegisterComponent<IProjectilesService>(_projectilesService);

            builder.RegisterInstance(_playerSettings).As<IPlayerSettings>();
            builder.RegisterInstance(_asteroidsSettings).As<IAsteroidsSettings>();
            builder.RegisterInstance(_projectilesSettings).As<IProjectilesSettings>();
            builder.RegisterInstance(_gameSettings).As<IGameSettings>();
            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<PlayerController>().As<IPlayerController>();
                entryPoints.Add<AsteroidsController>().As<IAsteroidsController>();
                entryPoints.Add<GameManager>().As<IGameManager>();
            });
        }

        protected override void OnDestroy()
        {
            _registrar.UnRegisterService<IPlayerInputService>();
            base.OnDestroy();
        }
    }
}
