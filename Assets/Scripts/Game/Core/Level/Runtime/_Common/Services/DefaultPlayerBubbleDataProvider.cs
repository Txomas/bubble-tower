using System;
using System.Linq;
using Common.Extensions;
using Game.Core.Bubbles;
using Zenject;

namespace Game.Core.Level.Runtime
{
    public class DefaultPlayerBubbleDataProvider : IPlayerBubbleDataProvider
    {
        [Inject] private readonly LevelModel _model;

        private Random _playerBubblesRandom;

        public BubbleData GetNewBubbleData()
        {
            _playerBubblesRandom ??= new Random();
            var color = _model.Bubbles.Values
                .Select(data => data.Color)
                .Distinct()
                .ToList()
                .GetRandom(_playerBubblesRandom);
            return new BubbleData(BubbleType.Default, color);
        }
    }
}