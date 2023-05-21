using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Services.Interfaces
{
    public interface ISpaceNavigationService
    {
        Vector3 GetCenterOfGameArea();
        Vector3 GetRandomPlaceInGameArea();
        void WrapAroundGameArea(ISpaceEntity entity);
        void RefreshGameAreaBounds();
    }
}