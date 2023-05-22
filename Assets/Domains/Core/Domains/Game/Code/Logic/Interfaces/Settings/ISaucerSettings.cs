using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Settings.Behaviors;

namespace Migs.Asteroids.Game.Logic.Interfaces.Settings
{
    public interface ISaucerSettings
    {
        int MaxSaucersOnScreen { get; }
        int MinScoreToAppear { get; }
        float CooldownTime { get; }
        float AppearanceChance { get; }
        public SaucerBehavior[] SaucerBehaviors { get; }
    }
}