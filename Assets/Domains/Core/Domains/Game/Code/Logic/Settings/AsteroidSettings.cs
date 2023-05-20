using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Data;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(AsteroidSettings), menuName = MenuItems.Settings + nameof(AsteroidSettings))]
    public class AsteroidSettings : ScriptableObject
    {
        [field:SerializeField] public AsteroidData[] AsteroidLevels { get; set; }
    }
}