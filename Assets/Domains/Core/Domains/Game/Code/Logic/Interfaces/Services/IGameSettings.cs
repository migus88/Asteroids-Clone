namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface IGameSettings
    {
        int RoundChangeDuration { get; }
        int ImmunityDurationOnRoundStart { get; }
        int PointsNeededForAdditionalLife { get; }
    }
}