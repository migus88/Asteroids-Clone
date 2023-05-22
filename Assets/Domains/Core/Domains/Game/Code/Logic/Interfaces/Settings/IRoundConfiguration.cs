using Migs.Asteroids.Game.Data;

namespace Migs.Asteroids.Game.Logic.Interfaces.Settings
{
    public interface IRoundConfiguration
    {
        int MinScore { get; set; }
        AsteroidRoundData[] Asteroids { get; set; }
    }
}