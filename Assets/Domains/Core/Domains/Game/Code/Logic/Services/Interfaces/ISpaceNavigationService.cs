using Migs.Asteroids.Game.Logic.Interfaces.Entities;

namespace Migs.Asteroids.Game.Logic.Services.Interfaces
{
    public interface ISpaceNavigationService
    {
        void WrapAroundGameArea(ISpaceEntity entity);
        void RefreshGameAreaBounds();
    }
}