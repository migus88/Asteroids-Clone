using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Settings;
using Migs.Asteroids.Game.Logic.Utils;
using UnityEngine;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class PlayerController : ITickable, IFixedTickable
    {
        private readonly IPlayer _player;
        private readonly IPlayerInputService _inputService;
        private readonly PlayerSettings _playerSettings;
        private readonly IViewportService _viewportService;
        private readonly (Vector2 BottomLeft, Vector2 TopRight) _screenEdges;

        private bool _shouldAddForce = false;

        public PlayerController(IPlayer player, IPlayerInputService inputService, PlayerSettings playerSettings,
            IViewportService viewportService)
        {
            _player = player;
            _inputService = inputService;
            _playerSettings = playerSettings;
            _viewportService = viewportService;
            _screenEdges = _viewportService.GetScreenEdges();

            _player.SetDrag(_playerSettings.VelocityDropRate);
        }

        public void Tick()
        {
            HandleMovement();
            
            Rotate();
            Warp();
        }

        public void FixedTick()
        {
            Thrust();   
        }

        private void Warp()
        {
            var isVisible = _viewportService.IsVisibleInViewport(_player.Bounds);

            if (isVisible)
            {
                return;
            }

            var newPosition =
                VectorUtils.WrapPosition(_player.Position, _player.Bounds, _screenEdges.BottomLeft, _screenEdges.TopRight);
            _player.Teleport(newPosition);
        }

        private void Rotate()
        {
            if (_inputService.IsRotationAxisPressed)
            {
                _player.Rotate(_inputService.RotationAxis, _playerSettings.RotationSpeed);
            }
        }

        private void HandleMovement() => _shouldAddForce = _inputService.IsAccelerationPressed;

        private void Thrust()
        {
            if (!_shouldAddForce)
            {
                return;
            }
            
            _player.AddForce(_playerSettings.ThrustersForce);
        }
    }
}