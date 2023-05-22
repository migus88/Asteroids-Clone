using System;
using Migs.Asteroids.Core.Logic.Utils;

namespace Migs.Asteroids.UI.UI.Code.Logic.Interfaces.Services
{
    public interface IMainMenuService
    {
        SceneAssetReference GameDomainReference { get; }
        
        event Action PlayButtonClicked;

        void Hide();
        void Show();
        void SetPlayButtonVisibility(bool isVisible);
    }
}