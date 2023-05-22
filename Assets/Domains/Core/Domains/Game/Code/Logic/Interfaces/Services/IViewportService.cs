using Migs.Asteroids.Game.Data;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface IViewportService
    {
        Rectangle GetScreenArea();
        bool IsVisibleInViewport(Bounds objectBounds);
    }
}