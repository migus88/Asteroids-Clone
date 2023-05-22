using System;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Migs.Asteroids.UI.UI.Code.View.Services
{
    public class GameUiService : MonoBehaviour, IGameUiService
    {
        private const string ScoreTextName = "score-text";
        private const string LivesTextName = "lives-text";
        private const string GameOverTextName = "game-over-text";
        
        public event Action RestartGamePressed;

        [SerializeField] private UIDocument _gameMenuDocument;
        [SerializeField] private UIDocument _gameOverDocument;

        private Label _scoreText;
        private Label _livesText;
        private Label _gameOverText;
        
        private void Awake()
        {
            _scoreText = _gameMenuDocument.rootVisualElement.Q<Label>(ScoreTextName);
            _livesText = _gameMenuDocument.rootVisualElement.Q<Label>(LivesTextName);
            _gameOverText = _gameOverDocument.rootVisualElement.Q<Label>(GameOverTextName);
            
            _gameOverText.AddManipulator(new Clickable(OnGameOverClicked));
        }

        public void ShowGameMenuPanel()
        {
            _gameMenuDocument.rootVisualElement.visible = true;
        }

        public void HideGameMenuPanel()
        {
            _gameMenuDocument.rootVisualElement.visible = false;
        }

        public void ShowGameOverPanel()
        {
            _gameOverDocument.rootVisualElement.visible = true;
        }

        public void HideGameOverPanel()
        {
            _gameOverDocument.rootVisualElement.visible = false;
        }

        public void SetScore(int score)
        {
            _scoreText.text = $"SCORE: {score}";
        }

        public void SetLives(int lives)
        {
            _livesText.text = $"LIVES: {lives}";
        }
        
        private void OnGameOverClicked()
        {
            RestartGamePressed?.Invoke();
        }
    }
}