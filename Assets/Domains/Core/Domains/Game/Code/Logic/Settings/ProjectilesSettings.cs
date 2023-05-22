using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Migs.Asteroids.Game.View.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(ProjectilesSettings), menuName = MenuItems.Settings + nameof(ProjectilesSettings))]
    public class ProjectilesSettings : ScriptableObject, IProjectilesSettings
    {
        // In a more complex game, each enemy will have a projectile with unique properties.
        // In this game, we want to achieve some gameplay balance, so there is a single configuration for projectiles
        
        [field:SerializeField] public ComponentReference<Projectile> PrefabReference { get; set; }
        [field:SerializeField] public float Speed { get; set; }
        [field:SerializeField] public float Lifetime { get; set; }
        [field:SerializeField] public int PreloadAmount { get; set; }
    }
}