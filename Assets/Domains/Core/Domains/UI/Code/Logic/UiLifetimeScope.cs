using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.UI.UI.Code.Logic.Controllers;
using Migs.Asteroids.UI.UI.Code.Logic.Interfaces;
using Migs.Asteroids.UI.UI.Code.Logic.Interfaces.Services;
using Migs.Asteroids.UI.UI.Code.View.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Migs.Asteroids.UI.UI.Code.Logic
{
    public class UiLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainMenuService _mainMenuService;
        [SerializeField] private GameUiService _gameUiService;
        

        private ICrossDomainServiceRegistrar _registrar;
        
        protected override void Configure(IContainerBuilder builder)
        {
            _registrar = Parent.Container.Resolve<ICrossDomainServiceRegistrar>();
            _registrar.RegisterComponentService<IGameUiService, GameUiService>(builder, this, _gameUiService);
            
            builder.RegisterComponent(_mainMenuService).As<IMainMenuService>();
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<MainMenuController>().As<IMainMenuController>();
            });
        }
    }
}
