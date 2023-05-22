using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Migs.Asteroids.Game.Logic.Settings.Behaviors;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(SaucerSettings), menuName = MenuItems.Settings + nameof(SaucerSettings))]
    public class SaucerSettings : ScriptableObject, ISaucerSettings
    {
        [field:SerializeField] public int MaxSaucersOnScreen { get; set; }
        [field:SerializeField] public int MinScoreToAppear { get; set; }
        [field:SerializeField] public float CooldownTime { get; set; }
        [field:SerializeField, Range(0,1f)] public float AppearanceChance { get; set; }
        [field:SerializeField] public SaucerBehavior[] SaucerBehaviors { get; set; }
    }
}