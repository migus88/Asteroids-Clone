using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
    public class GameManager : IGameManager, IAsyncStartable, ITickable, IDisposable
    {
        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                if (_currentScore != value)
                {
                    ScoreChanged?.Invoke(_currentScore, value);
                    HandleLifeAccumulation(_currentScore, value);
                }

                _currentScore = value;
            }
        }

        public event ScoreChange ScoreChanged;

        private int _currentScore;
        private bool _isRoundRunning;
        private bool _isGameRunning;

        private readonly IGameSettings _gameSettings;
        private readonly IPlayerController _playerController;
        private readonly IAsteroidsController _asteroidsController;
        private readonly IAsteroidsSettings _asteroidsSettings;
        private readonly IRoundsService _roundsService;
        private readonly ISpaceNavigationService _spaceNavigationService;

        public GameManager(IGameSettings gameSettings, IPlayerController playerController,
            IAsteroidsController asteroidsController, IAsteroidsSettings asteroidsSettings,
            IRoundsService roundsService, ISpaceNavigationService spaceNavigationService)
        {
            _gameSettings = gameSettings;
            _playerController = playerController;
            _asteroidsController = asteroidsController;
            _asteroidsSettings = asteroidsSettings;
            _roundsService = roundsService;
            _spaceNavigationService = spaceNavigationService;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.WhenAll(
                _playerController.Init(),
                _asteroidsController.Init(),
                _roundsService.Init()
            );

            RunGame().Forget();
        }

        public void Tick()
        {
        }

        public async UniTaskVoid RunGame()
        {
            if (_isGameRunning)
            {
                Debug.LogError("Trying to run multiple instances of the game");
                return;
            }

            _isGameRunning = true;

            _playerController.Reset();

            RoundResult roundResult;
            do
            {
                roundResult = await RunRound();
            } while (roundResult != RoundResult.Defeat);

            _isGameRunning = false;
            // TODO: Show game over window
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

            while (_playerController.Lives >= 0 && _asteroidsController.SpawnedAsteroids > 0)
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
            var roundConfig = _roundsService.GetRoundConfiguration(CurrentScore);

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

        private void HandleLifeAccumulation(int previousScore, int newScore)
        {
            if (newScore < _gameSettings.PointsNeededForAdditionalLife)
            {
                return;
            }
            
            var previousValue = previousScore / _gameSettings.PointsNeededForAdditionalLife;
            var newValue = newScore / _gameSettings.PointsNeededForAdditionalLife;

            _playerController.Lives += newValue - previousValue;
        }

        public void Dispose()
        {
        }
    }
}