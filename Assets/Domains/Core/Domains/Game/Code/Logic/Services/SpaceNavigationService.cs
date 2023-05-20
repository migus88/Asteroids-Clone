using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Utils;

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