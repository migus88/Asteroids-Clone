using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings.Behaviors
{
    [CreateAssetMenu(fileName = nameof(BigSaucerBehavior), menuName = MenuItems.Settings + nameof(BigSaucerBehavior))]
    public class BigSaucerBehavior : SaucerBehavior
    {
        protected override Quaternion GetProjectileDirection(ISaucer saucer, IPlayer player) => Quaternion.Euler(0, Random.Range(0, 360f), 0);
    }
}