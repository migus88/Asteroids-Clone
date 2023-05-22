namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface ISoundService
    {
        void PlaySpaceshipThrusters();
        void StopSpaceshipThrusters();
        void PlaySpaceshipLaser();
        void PlaySpaceshipExplosion();
        
        void PlaySaucerThrusters();
        void StopSaucerThrusters();
        void PlaySaucerLaser();
        void PlaySaucerExplosion();

        void PlayAsteroidExplosion();
    }
}