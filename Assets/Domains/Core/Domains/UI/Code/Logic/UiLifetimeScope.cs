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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_mainMenuService).As<IMainMenuService>();
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<PanelsController>().As<IPanelsController>();
            });
        }
    }
}
