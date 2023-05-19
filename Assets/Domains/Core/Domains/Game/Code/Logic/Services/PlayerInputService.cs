using System;
using Migs.Asteroids.Core.Logic.Enums;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Input;
using UnityEngine;
using VContainer;

namespace Migs.Asteroids.Game.Logic.Services
{
    internal class PlayerInputService : IPlayerInputService, IDisposable
    {
        public float RotationAxis => _gameplayInput.Player.Rotation.ReadValue<float>();
        public bool IsRotationAxisPressed => RotationAxis != 0;
        public bool IsAccelerationPressed => _gameplayInput.Player.Acceleration.IsPressed();
        public bool IsHyperspacePressed => _gameplayInput.Player.Hyperspace.IsPressed();
        

        private readonly DefaultGameplayInput _gameplayInput;
        private readonly IApplicationService _applicationService;

        public PlayerInputService(DefaultGameplayInput gameplayInput, IApplicationService applicationService)
        {
            _gameplayInput = gameplayInput;
            _gameplayInput.Enable();
            
            _applicationService = applicationService;
            _applicationService.ApplicationStateChanged += OnApplicationStateChanged;
            
        }

        private void OnApplicationStateChanged(ApplicationState state)
        {
            switch (state)
            {
                case ApplicationState.Active when !_gameplayInput.asset.enabled:
                    _gameplayInput.Enable();
                    break;
                case ApplicationState.Inactive:
                    _gameplayInput.Disable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void Dispose()
        {
            _applicationService.ApplicationStateChanged -= OnApplicationStateChanged;
            _gameplayInput?.Dispose();
        }
    }
}