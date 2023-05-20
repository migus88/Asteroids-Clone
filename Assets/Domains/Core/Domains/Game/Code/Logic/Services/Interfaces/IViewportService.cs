using Migs.Asteroids.Game.Data;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Services.Interfaces
{
    public interface IViewportService
    {
        Rectangle GetScreenArea();
        bool IsVisibleInViewport(Bounds objectBounds);
    }
}