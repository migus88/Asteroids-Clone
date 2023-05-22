namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface IGameSettings
    {
        int RoundChangeDuration { get; }
        int ImmunityDuration { get; }
    }
}