using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Settings;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class PlayerController : SpaceEntityController
    {
        private readonly IPlayer _player;
        private readonly IPlayerInputService _inputService;
        private readonly PlayerSettings _playerSettings;

        private bool _shouldAddForce = false;

        public PlayerController(IPlayer player, IPlayerInputService inputService, PlayerSettings playerSettings,
            IViewportService viewportService) : base(viewportService, player)
        {
            _player = player;
            _inputService = inputService;
            _playerSettings = playerSettings;

            _player.SetDrag(_playerSettings.VelocityDropRate);
        }

        public override void Tick()
        {
            HandleMovement();
            Rotate();
            
            base.Tick();
        }

        public override void FixedTick()
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