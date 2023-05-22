namespace Migs.Asteroids.Game.Logic.Interfaces.Controllers
{
    public interface ISaucerController : IController
    {
        void Enable();
        void Disable();
        void Reset();
    }
}