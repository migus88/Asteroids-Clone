using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(GameSettings), menuName = MenuItems.Settings + nameof(GameSettings))]
    public class GameSettings : ScriptableObject, IGameSettings
    {
        [field:SerializeField] public int RoundChangeDuration { get; set; }
        [field:SerializeField] public int ImmunityDurationOnRoundStart { get; set; }
        [field:SerializeField] public int PointsNeededForAdditionalLife { get; set; }
    }
}