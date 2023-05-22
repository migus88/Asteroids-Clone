using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Utils;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Handlers
{
    public class SpaceNavigationService : ISpaceNavigationService
    {
        private readonly IViewportService _viewportService;
        private Rectangle _gameArea;

        public SpaceNavigationService(IViewportService viewportService)
        {
            _viewportService = viewportService;
            
            // Caching area can cause issues if the screen size changes
            RefreshGameAreaBounds();
        }

        public void RefreshGameAreaBounds()
        {
            _gameArea = _viewportService.GetScreenArea();
        }

        public Vector3 GetCenterOfGameArea()
        {
            const float half = 0.5f;
            var x = _gameArea.MaxX - (_gameArea.MaxX - _gameArea.MinX) * half;
            var z = _gameArea.MaxY - (_gameArea.MaxY - _gameArea.MinY) * half;
            
            return new Vector3(x, 0, z);
        }

        public Vector3 GetRandomPlaceInGameArea()
        {
            var x = Random.Range(_gameArea.MinX, _gameArea.MaxX);
            var z = Random.Range(_gameArea.MinY, _gameArea.MaxY);
            return new Vector3(x, 0, z);
        }

        public void WrapAroundGameArea(ISpaceEntity entity)
        {
            var isVisible = _viewportService.IsVisibleInViewport(entity.Bounds);

            if (isVisible)
            {
                return;
            }

            entity.Position = VectorUtils.WrapPosition(entity.Position, entity.Bounds, _gameArea);
        }
    }
}