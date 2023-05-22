using System;
using Cysharp.Threading.Tasks;
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

        private Clickable _clickManipulator;

        private void OnEnable()
        {
            InitAll();
        }

        // There is a strange bug in Unity, that returns rootVisualElement as null only in builds.
        // This method is a workaround for this bug.
        private void InitAll()
        {
            InitGameMenu();
            InitGameOver();
        }

        private void InitGameMenu()
        {
            if (_gameMenuDocument.rootVisualElement == null)
            {
                return;
            }

            _scoreText = _gameMenuDocument.rootVisualElement.Q<Label>(ScoreTextName);
            _livesText = _gameMenuDocument.rootVisualElement.Q<Label>(LivesTextName);
        }

        private void InitGameOver()
        {
            if (_gameOverDocument.rootVisualElement == null)
            {
                return;
            }

            _gameOverText = _gameOverDocument.rootVisualElement.Q<Label>(GameOverTextName);
            _clickManipulator = new Clickable(OnGameOverClicked);
            _gameOverText?.AddManipulator(_clickManipulator);
        }

        public void ShowGameMenuPanel()
        {
            _gameMenuDocument.gameObject.SetActive(true);
            InitGameMenu();
        }

        public void HideGameMenuPanel()
        {
            _gameMenuDocument.gameObject.SetActive(false);
        }

        public void ShowGameOverPanel()
        {
            _gameOverDocument.gameObject.SetActive(true);
            InitGameOver();
        }

        public void HideGameOverPanel()
        {
            if(_clickManipulator != null)
            {
                _gameOverText?.RemoveManipulator(_clickManipulator);
                _clickManipulator = null;
            }
            _gameOverDocument.gameObject.SetActive(false);
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