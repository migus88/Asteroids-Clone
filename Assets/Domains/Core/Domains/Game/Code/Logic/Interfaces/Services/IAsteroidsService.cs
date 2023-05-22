using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;

namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface IAsteroidsService
    {
        UniTask Preload(int amount = 0);
        IAsteroid GetAvailableAsteroid();
        void ReturnAsteroid(IAsteroid asteroid);
    }
}