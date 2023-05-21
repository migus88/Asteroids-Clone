using Migs.Asteroids.Core.Logic.Utils;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(PlayerSettings), menuName = MenuItems.Settings + nameof(PlayerSettings))]
    public class PlayerSettings : ScriptableObject
    {
        [field:Header("Movement")]
        [field:SerializeField] public float RotationSpeed { get; set; }
        [field:SerializeField] public float ThrustersForce { get; set; }
        [field:SerializeField] public float MaxVelocity { get; set; }
        [field:SerializeField] public float VelocityDropRate { get; set; }
        [field:SerializeField] public int HyperspaceDurationInSeconds { get; set; }
        
        [field:Header("Shooting")]
        [field:SerializeField] public float FireRate { get; set; }
        [field:SerializeField] public int MagazineSize { get; set; }
        
        [field:Header("Other")]
        [field:SerializeField] public int Lives { get; set; }
    }
}