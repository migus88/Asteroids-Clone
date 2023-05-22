using System;
using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(RoundConfiguration), menuName = MenuItems.Settings + nameof(RoundConfiguration))]
    public class RoundConfiguration : ScriptableObject, IRoundConfiguration
    {
        [field:SerializeField] public int MinScore { get; set; }
        [field:SerializeField] public AsteroidRoundData[] Asteroids { get; set; }
    }
}