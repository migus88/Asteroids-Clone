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

        public (Vector3 Position, Vector3 Direction) GetRandomSaucerSpawnPosition(float verticalOffset)
        {
            var horizontalPositions = new (float X, Vector3 Direction)[]
                { 
                    (_gameArea.MinX, Vector3.right), 
                    (_gameArea.MaxX, Vector3.left) 
                };
            
            var horizontalPositionIndex = Random.Range(0, horizontalPositions.Length);
            var horizontalPosition = horizontalPositions[horizontalPositionIndex];

            var x = horizontalPosition.X;
            var z = Random.Range(_gameArea.MinY + verticalOffset, _gameArea.MaxY - verticalOffset);

            return (new Vector3(x, 0, z), horizontalPosition.Direction);
        }

        public void WrapAroundGameArea(ISpaceEntity entity)
        {
            var isVisible = IsObjectOutOfGameArea(entity);

            if (isVisible)
            {
                return;
            }

            entity.Position = VectorUtils.WrapPosition(entity.Position, entity.Bounds, _gameArea);
        }

        public bool IsObjectOutOfGameArea(ISpaceEntity entity, float errorMargin = 0)
        {
            var bounds = new Bounds(entity.Bounds.center, entity.Bounds.size + new Vector3(errorMargin, errorMargin, errorMargin));
            return _viewportService.IsVisibleInViewport(bounds);
        }
    }
}