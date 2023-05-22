using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.View.Entities;

namespace Migs.Asteroids.Game.Logic.Interfaces.Settings
{
    public interface IProjectilesSettings
    {
        ComponentReference<Projectile> PrefabReference { get; set; }
        float Speed { get; set; }
        float Lifetime { get; set; }
        int PreloadAmount { get; set; }
    }
}