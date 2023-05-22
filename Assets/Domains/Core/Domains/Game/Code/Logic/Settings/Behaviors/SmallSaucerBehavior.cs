using Migs.Asteroids.Core.Logic.Utils;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Settings.Behaviors
{
    [CreateAssetMenu(fileName = nameof(SmallSaucerBehavior), menuName = MenuItems.Settings + nameof(SmallSaucerBehavior))]
    public class SmallSaucerBehavior : SaucerBehavior
    {
        protected override Quaternion GetProjectileDirection(ISaucer saucer, IPlayer player)
        {
            var direction = player.Position - saucer.Position;
            direction.y = 0;
            return Quaternion.LookRotation(direction);
        }
    }
}