using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Utils;
using UnityEngine;
using VContainer.Unity;

namespace Migs.Asteroids.Game.Logic.Controllers
{
    public abstract class SpaceEntityController : ITickable, IFixedTickable
    {
        protected readonly IViewportService _viewportService;
        protected readonly ISpaceEntity _spaceEntity;
        protected readonly Rectangle _gameArea;

        protected SpaceEntityController(IViewportService viewportService, ISpaceEntity spaceEntity)
        {
            _viewportService = viewportService;
            _spaceEntity = spaceEntity;
            
            // Caching area can cause issues if the screen size changes
            _gameArea = _viewportService.GetScreenArea();
        }
        
        public virtual void Tick()
        {
            Wrap();
        }

        public virtual void FixedTick() { }
        
        private void Wrap()
        {
            var isVisible = _viewportService.IsVisibleInViewport(_spaceEntity.Bounds);

            if (isVisible)
            {
                return;
            }

            _spaceEntity.Position = VectorUtils.WrapPosition(_spaceEntity.Position, _spaceEntity.Bounds, _gameArea);
        }
    }
}