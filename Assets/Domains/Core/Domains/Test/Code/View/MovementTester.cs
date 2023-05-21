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
            if (_playerInputService.IsRotationButtonPressed)
            {
                Debug.Log(_playerInputService.RotationAxis);
            }

            if (_playerInputService.IsAccelerationButtonPressed)
            {
                Debug.Log("Acceleration");
            }

            if (_playerInputService.IsHyperspaceButtonPressed)
            {
                Debug.Log("Hyperspace");
            }
        }
    }
}