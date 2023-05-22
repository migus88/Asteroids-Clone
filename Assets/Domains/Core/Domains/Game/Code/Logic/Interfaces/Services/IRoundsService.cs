using Migs.Asteroids.Game.Logic.Settings;

namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface IRoundsService : IAsyncInitializable
    {
        RoundConfiguration GetRoundConfiguration(int score);
    }
}