using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Cathei.LinqGen;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Migs.Asteroids.Game.Logic.Services
{
    public class RoundsService : IRoundsService
    {
        // In a real game (specially a live one) this wouldn't be a manual const.
        // The labels would be managed via editor script that would generate a class
        // with all of the labels or something similar. But implementing this as part
        // of the exam feels like an overkill.
        private const string ConfigurationLabel = "Round Configuration";

        private List<RoundConfiguration> _orderedConfigurations;

        public async UniTask Init()
        {
            var bag = new ConcurrentBag<RoundConfiguration>();

            await Addressables.LoadAssetsAsync<RoundConfiguration>(new List<string> { ConfigurationLabel }, config =>
            {
                bag.Add(config);
            }, Addressables.MergeMode.UseFirst);

            _orderedConfigurations = bag.Gen()
                .OrderBy(c => c.MinScore)
                .ToList();
        }

        public RoundConfiguration GetRoundConfiguration(int score) =>
            _orderedConfigurations
                .Gen()
                .Where(c => c.MinScore <= score)
                .LastOrDefault();
    }
}