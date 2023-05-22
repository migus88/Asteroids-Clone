using System;
using UnityEngine;

namespace Migs.Asteroids.Game.Data
{
    [Serializable]
    public class AsteroidRoundData
    {
        [field:SerializeField] public string Name { get; set; }
        [field:SerializeField] public int Level { get; set; }
        [field:SerializeField] public int Amount { get; set; }
        [field:SerializeField] public float SpeedMultiplier { get; set; }
        
    }
}