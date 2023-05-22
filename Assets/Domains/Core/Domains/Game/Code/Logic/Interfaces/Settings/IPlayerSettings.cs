namespace Migs.Asteroids.Game.Logic.Interfaces.Settings
{
    public interface IPlayerSettings
    {
        float RotationSpeed { get; set; }
        float ThrustersForce { get; set; }
        float MaxVelocity { get; set; }
        float VelocityDropRate { get; set; }
        int HyperspaceDurationInSeconds { get; set; }
        float FireRate { get; set; }
        int MagazineSize { get; set; }
        int Lives { get; set; }
        int ImmunityDurationOnRespawn { get; }
    }
}