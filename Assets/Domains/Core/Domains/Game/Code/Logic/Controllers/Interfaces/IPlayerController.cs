using System;

namespace Migs.Asteroids.Game.Logic.Controllers.Interfaces
{
    public interface IPlayerController
    {
        event PlayerExplosion Exploded;
        
        int Lives { get; }

        void Reset();
        void Enable();
        void Disable();
    }

    public delegate void PlayerExplosion(bool hasMoreLives);
}