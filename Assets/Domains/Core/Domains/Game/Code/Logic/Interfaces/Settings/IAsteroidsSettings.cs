using Migs.Asteroids.Game.Data;

namespace Migs.Asteroids.Game.Logic.Interfaces.Settings
{
    public interface IAsteroidsSettings
    {
        AsteroidData[] AsteroidLevels { get; set; }
    }
}