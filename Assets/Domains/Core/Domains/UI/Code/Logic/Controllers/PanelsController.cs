using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.UI.UI.Code.Logic.Interfaces;
using Migs.Asteroids.UI.UI.Code.Logic.Interfaces.Services;
using VContainer.Unity;

namespace Migs.Asteroids.UI.UI.Code.Logic.Controllers
{
    public class PanelsController : IPanelsController, IInitializable, IDisposable
    {
        private readonly IMainMenuService _mainMenuService;
        private readonly IApplicationService _applicationService;

        public PanelsController(IMainMenuService mainMenuService, IApplicationService applicationService)
        {
            _mainMenuService = mainMenuService;
            _applicationService = applicationService;
        }
        
        public void Initialize()
        {
            _mainMenuService.PlayButtonClicked += OnPlayButtonClicked;
        }

        private void OnPlayButtonClicked()
        {
            LoadGame().Forget();
        }

        private async UniTaskVoid LoadGame()
        {
            _mainMenuService.SetPlayButtonVisibility(false);
            await _applicationService.LoadDomain(_mainMenuService.GameDomainReference);
            _mainMenuService.Hide();
        }

        public void Dispose()
        {
            _mainMenuService.PlayButtonClicked -= OnPlayButtonClicked;
        }
    }
}