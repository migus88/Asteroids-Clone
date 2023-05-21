using UnityEngine;

namespace Migs.Asteroids.Core.Logic.Services.Interfaces
{
    public interface IPlayerInputService
    {
        float RotationAxis { get; }
        bool IsRotationAxisPressed { get; }
        bool IsAccelerationPressed { get; }
        bool IsHyperspacePressed { get; }
        bool IsShootingPressed { get; }
    }
}