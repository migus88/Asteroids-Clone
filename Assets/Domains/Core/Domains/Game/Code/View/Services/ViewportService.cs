using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Utils;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Services
{
    public class ViewportService : MonoBehaviour, IViewportService
    {
        [SerializeField] private Camera _camera;

        public bool IsVisibleInViewport(Bounds objectBounds)
        {
            var minPoint = _camera.WorldToViewportPoint(objectBounds.min);
            var maxPoint = _camera.WorldToViewportPoint(objectBounds.max);

            var objectRect = Rect.MinMaxRect(minPoint.x, minPoint.y, maxPoint.x, maxPoint.y);
            var viewportRect = new Rect(0, 0, 1, 1);

            return viewportRect.Overlaps(objectRect);
        }
        
        public Rectangle GetScreenArea()
        {
            var bottomLeft = _camera.ScreenToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            var topRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.nearClipPlane));

            return new Rectangle(bottomLeft.ToPosition2D(), topRight.ToPosition2D());
        }
    }
}