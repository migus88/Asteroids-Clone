using System;
using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.UI.UI.Code.Logic.Interfaces.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace Migs.Asteroids.UI.UI.Code.View.Services
{
    public class MainMenuService : MonoBehaviour, IMainMenuService
    {
        private const string StartButtonName = "start-button";

        public event Action PlayButtonClicked;
        
        [field:SerializeField] public SceneAssetReference GameDomainReference { get; set; }

        [SerializeField] private UIDocument _uiDocument;

        private Label _playButton;

        private void Awake()
        {
            _playButton = _uiDocument.rootVisualElement.Q<Label>(StartButtonName);
            _playButton.AddManipulator(new Clickable(OnPlayButtonClicked));
        }

        private void OnPlayButtonClicked()
        {
            PlayButtonClicked?.Invoke();
        }

        public void Hide()
        {
            _uiDocument.rootVisualElement.visible = false;
        }

        public void Show()
        {
            _uiDocument.rootVisualElement.visible = true;
        }

        public void SetPlayButtonVisibility(bool isVisible)
        {
            _playButton.visible = isVisible;
        }

    }
}