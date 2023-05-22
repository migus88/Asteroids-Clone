using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface ISpaceNavigationService
    {
        Vector3 GetCenterOfGameArea();
        Vector3 GetRandomPlaceInGameArea();
        (Vector3 Position, Vector3 Direction) GetRandomSaucerSpawnPosition(float verticalOffset);
        void WrapAroundGameArea(ISpaceEntity entity);
        void RefreshGameAreaBounds();
        bool IsObjectOutOfGameArea(ISpaceEntity entity, float errorMargin = 0);
    }
}