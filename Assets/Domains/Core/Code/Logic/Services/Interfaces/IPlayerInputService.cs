using UnityEngine;

namespace Migs.Asteroids.Core.Logic.Services.Interfaces
{
    public interface IPlayerInputService
    {
        /// <summary>
        /// Normalized rotation axis
        /// </summary>
        float RotationAxis { get; }
        /// <summary>
        /// Is any of the rotation buttons in a press state
        /// </summary>
        bool IsRotationButtonPressed { get; }
        /// <summary>
        /// Is acceleration button is in a press state
        /// </summary>
        bool IsAccelerationButtonPressed { get; }
        /// <summary>
        /// Is acceleration button was first pressed during this tick
        /// </summary>
        bool IsAccelerationButtonPressStarted { get; }
        /// <summary>
        /// Is hyperspace button is in a press state
        /// </summary>
        bool IsHyperspaceButtonPressed { get; }
        /// <summary>
        /// Is hyperspace button was first pressed during this tick
        /// </summary>
        bool IsHyperspaceButtonPressStarted { get; }
        /// <summary>
        /// Is shooting button is in a press state
        /// </summary>
        bool IsShootingButtonPressed { get; }
        /// <summary>
        /// Is shooting button was first pressed during this tick
        /// </summary>
        bool IsShootingButtonPressStarted { get; }
    }
}