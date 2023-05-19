using System;
using Migs.Asteroids.Core.Logic.Services;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Domains.Core.Domains.Test.Code.View
{
    public class MovementTester : ITickable
    {
        private readonly IPlayerInputService _playerInputService;

        public MovementTester(ICrossDomainServiceResolver resolver)
        {
            _playerInputService = resolver.ResolveService<IPlayerInputService>();
        }

        public void Tick()
        {
            if (_playerInputService.IsRotationAxisPressed)
            {
                Debug.Log(_playerInputService.RotationAxis);
            }

            if (_playerInputService.IsAccelerationPressed)
            {
                Debug.Log("Acceleration");
            }

            if (_playerInputService.IsHyperspacePressed)
            {
                Debug.Log("Hyperspace");
            }
        }
    }
}