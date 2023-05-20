using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Services.Interfaces
{
    public interface IViewportService
    {
        (Vector2 BottomLeft, Vector2 TopRight) GetScreenEdges();
        bool IsVisibleInViewport(Bounds objectBounds);
    }
}