using Migs.Asteroids.Core.Logic.Utils;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(PlayerSettings), menuName = MenuItems.Settings + nameof(PlayerSettings))]
    public class PlayerSettings : ScriptableObject
    {
        [field:SerializeField] public float RotationSpeed { get; set; }
        [field:SerializeField] public float MaxVelocity { get; set; }
        [field:SerializeField] public float ThrustersForce { get; set; }
        [field:SerializeField] public float VelocityDropRate { get; set; }
    }
}