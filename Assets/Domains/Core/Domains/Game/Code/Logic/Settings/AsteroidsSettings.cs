using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(AsteroidsSettings), menuName = MenuItems.Settings + nameof(AsteroidsSettings))]
    public class AsteroidsSettings : ScriptableObject, IAsteroidsSettings
    {
        [field:SerializeField] public AsteroidData[] AsteroidLevels { get; set; }
    }
}