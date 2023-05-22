using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Managers;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Migs.Asteroids.Game.Logic.Settings;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Migs.Asteroids.Game.Logic.Managers
{
    public class GameManager : IGameManager, IAsyncStartable, IDisposable
    {
        private bool _isRoundRunning;
        private bool _isGameRunning;

        private readonly IGameSettings _gameSettings;
        private readonly IPlayerController _playerController;
        private readonly IAsteroidsController _asteroidsController;
        private readonly IAsteroidsSettings _asteroidsSettings;
        private readonly IRoundsService _roundsService;
        private readonly ISpaceNavigationService _spaceNavigationService;
        private readonly IScoreService _scoreService;
        private readonly ISaucerController _saucerController;
        private readonly IGameUiService _gameUiService;

        public GameManager(IGameSettings gameSettings, IPlayerController playerController,
            IAsteroidsController asteroidsController, IAsteroidsSettings asteroidsSettings,
            IRoundsService roundsService, ISpaceNavigationService spaceNavigationService, IScoreService scoreService,
            ISaucerController saucerController, ICrossDomainServiceResolver crossDomainServiceResolver)
        {
            _gameSettings = gameSettings;
            _playerController = playerController;
            _asteroidsController = asteroidsController;
            _asteroidsSettings = asteroidsSettings;
            _roundsService = roundsService;
            _spaceNavigationService = spaceNavigationService;
            _scoreService = scoreService;
            _saucerController = saucerController;
            _gameUiService = crossDomainServiceResolver.ResolveService<IGameUiService>();
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.WhenAll(
                _playerController.Init(),
                _asteroidsController.Init(),
                _roundsService.Init(),
                _saucerController.Init()
            );

            _scoreService.ScoreChanged += OnScoreChanged;
            _gameUiService.RestartGamePressed += OnRestartPressed;

            RunGame().Forget();
        }

        public async UniTaskVoid RunGame()
        {
            if (_isGameRunning)
            {
                Debug.LogError("Trying to run multiple instances of the game");
                return;
            }

            _isGameRunning = true;
            
            _gameUiService.ShowGameMenuPanel();
            _gameUiService.HideGameOverPanel();

            _playerController.Reset();
            _saucerController.Reset();
            
            _gameUiService.SetLives(_playerController.Lives);

            var roundResult = RoundResult.None;
            
            while (roundResult != RoundResult.Defeat)
            {
                roundResult = await RunRound();
            } 

            _isGameRunning = false;
            
            _gameUiService.ShowGameOverPanel();
        }

        private async UniTask<RoundResult> RunRound()
        {
            if (_isRoundRunning)
            {
                Debug.LogWarning("Can't run multiple rounds at the same time");
                return RoundResult.None;
            }

            _isRoundRunning = true;

            await UniTask.Delay(TimeSpan.FromSeconds(_gameSettings.RoundChangeDuration));

            SetupRound();

            _playerController.MakePlayerImmuneToDamage(_gameSettings.ImmunityDurationOnRoundStart);
            
            _playerController.Enable();
            _saucerController.Enable();

            while (_playerController.Lives > 0 && _asteroidsController.SpawnedAsteroids > 0)
            {
                await UniTask.Yield();

                // TODO: Implement pause
            }

            _playerController.Disable();
            _isRoundRunning = false;

            var isPlayerWon = _playerController.Lives >= 0 && _asteroidsController.SpawnedAsteroids == 0;
            return isPlayerWon ? RoundResult.Win : RoundResult.Defeat;
        }

        private void SetupRound()
        {
            var roundConfig = _roundsService.GetRoundConfiguration(_scoreService.CurrentScore);

            foreach (var asteroid in roundConfig.Asteroids)
            {
                for (var i = 0; i < asteroid.Amount; i++)
                {
                    var position = _spaceNavigationService.GetRandomPlaceInGameArea();
                    var rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                    _asteroidsController.SpawnAsteroid(asteroid.Level, position, rotation, asteroid.SpeedMultiplier);
                }
            }
        }

        private void OnScoreChanged(int oldScore, int newScore)
        {
            Debug.Log($"New Score: {newScore}");
            HandleLifeAccumulation(oldScore, newScore);
            _gameUiService.SetScore(newScore);
        }

        private void HandleLifeAccumulation(int previousScore, int newScore)
        {
            if (newScore < _gameSettings.PointsNeededForAdditionalLife)
            {
                return;
            }
            
            var previousValue = previousScore / _gameSettings.PointsNeededForAdditionalLife;
            var newValue = newScore / _gameSettings.PointsNeededForAdditionalLife;

            _playerController.Lives += newValue - previousValue;
            _gameUiService.SetLives(_playerController.Lives);
        }

        private void OnRestartPressed()
        {
            RunGame().Forget();
        }

        public void Dispose()
        {
            _scoreService.ScoreChanged -= OnScoreChanged;
            _gameUiService.RestartGamePressed -= OnRestartPressed;
        }
    }
}