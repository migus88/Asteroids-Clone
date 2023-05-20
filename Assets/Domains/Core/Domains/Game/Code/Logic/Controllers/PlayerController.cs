using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Settings;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public class PlayerController : ITickable
    {
        private readonly IPlayer _player;
        private readonly IPlayerInputService _inputService;
        private readonly PlayerSettings _playerSettings;

        public PlayerController(IPlayer player, IPlayerInputService inputService, PlayerSettings playerSettings)
        {
            _player = player;
            _inputService = inputService;
            _playerSettings = playerSettings;

            _player.SetDrag(_playerSettings.VelocityDropRate);
        }

        public void Tick()
        {
            if (_inputService.IsRotationAxisPressed)
            {
                _player.Rotate(_inputService.RotationAxis, _playerSettings.RotationSpeed);
            }
            
            if (_inputService.IsAccelerationPressed && _player.Velocity.magnitude < _playerSettings.MaxVelocity)
            {
                _player.AddForce(_playerSettings.ThrustersForce);
            }
            
        }
    }
}