using System;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.UI.UI.Code.Logic.Interfaces;
using Migs.Asteroids.UI.UI.Code.Logic.Interfaces.Services;
using VContainer.Unity;

namespace Migs.Asteroids.UI.UI.Code.Logic.Controllers
{
    public class MainMenuController : IMainMenuController, IInitializable, IDisposable
    {
        private readonly IMainMenuService _mainMenuService;
        private readonly IApplicationService _applicationService;
        private readonly IGameUiService _gameUiService;

        public MainMenuController(IMainMenuService mainMenuService, IApplicationService applicationService, IGameUiService gameUiService)
        {
            _mainMenuService = mainMenuService;
            _applicationService = applicationService;
            _gameUiService = gameUiService;
        }
        
        public void Initialize()
        {
            _gameUiService.HideGameMenuPanel();
            _gameUiService.HideGameOverPanel();
            
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