using Domains.Core.Domains.Test.Code.View;
using Migs.Asteroids.Core.Logic.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Migs.Asteroids.Test
{
    public class TestLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MovementTester>();
        }
    }
}
