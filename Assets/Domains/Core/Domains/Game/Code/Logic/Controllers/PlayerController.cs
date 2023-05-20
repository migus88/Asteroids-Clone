using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Handlers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Settings;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class PlayerController : ITickable, IFixedTickable
    {
        private readonly IPlayer _player;
        private readonly IPlayerInputService _inputService;
        private readonly PlayerSettings _playerSettings;
        private readonly ISpaceNavigationService _spaceNavigationService;

        private bool _shouldAddForce = false;

        public PlayerController(IPlayer player, IPlayerInputService inputService, PlayerSettings playerSettings, ISpaceNavigationService spaceNavigationService)
        {
            _player = player;
            _inputService = inputService;
            _playerSettings = playerSettings;
            _spaceNavigationService = spaceNavigationService;

            _player.SetDrag(_playerSettings.VelocityDropRate);
        }

        public void Tick()
        {
            HandleMovement();
            Rotate();
            
            _spaceNavigationService.WrapAroundGameArea(_player);
        }

        public void FixedTick()
        {
            Thrust();   
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