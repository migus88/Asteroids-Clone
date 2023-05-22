namespace Migs.Asteroids.Game.Logic.Interfaces.Controllers
{
    public interface IPlayerController : IController
    {
        event PlayerExplosion Exploded;
        
        int Lives { get; set; }

        void Reset();
        void Enable();
        void Disable();
        void MakePlayerImmuneToDamage(int durationInSeconds);
    }

    public delegate void PlayerExplosion(bool hasMoreLives);
}