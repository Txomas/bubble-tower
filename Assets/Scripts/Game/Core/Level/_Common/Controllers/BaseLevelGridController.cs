using System.Collections.Generic;
using Game.Core.Bubbles;
using UnityEngine;
using Zenject;
using Zenject.Helpers;
using Object = UnityEngine.Object;

namespace Game.Core.Level
{
    public abstract class BaseLevelGridController : BaseController
    {
        [Inject] private readonly BubblesConfig _bubblesConfig;
        [Inject] private readonly LevelGridConfig _config;
        [Inject] private readonly IGridModel _model;
        [Inject] private readonly IGridService _service;
        [Inject] private readonly LevelGridView _view;
        private readonly Dictionary<Vector2Int, BubbleView> _bubbles = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Subscribe<NewLevelGridSet>(Rebuild);
            Subscribe<BubbleChanged>(OnBubbleChanged);
            
            _view.BubbleAdded += AddBubble;
        }
        
        protected void Rebuild()
        {
            foreach (var bubble in _bubbles.Values)
            {
                Object.Destroy(bubble.gameObject);
            }
            
            _bubbles.Clear();

            var rows = _config.Rows;
            var columns = _config.Columns;
            
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    var indexes = new Vector2Int(c, r);
                    
                    if (ShouldCreateBubble(indexes, out var data))
                    {
                        CreateBubble(indexes, data);
                    }
                }
            }
        }

        protected virtual bool ShouldCreateBubble(Vector2Int index, out BubbleData data)
        {
            return _model.Bubbles.TryGetValue(index, out data);
        }

        private void CreateBubble(Vector2Int index, BubbleData data)
        {
            var bubble = _bubblesConfig.CreateBubble(data);
            
            _view.AddBubble(bubble, index);
            bubble.transform.localPosition = _service.IndexToLocalPos(index);
        }

        private void AddBubble(BubbleView bubbleView)
        {
            var index = bubbleView.GridIndex;
            RemoveBubble(index);
            _bubbles.Add(index, bubbleView);
        }

        private void OnBubbleChanged(BubbleChanged changedData)
        {
            UpdateBubbleColor(changedData.Index, changedData.Data);
        }
        
        protected void UpdateBubbleColor(Vector2Int index, BubbleData data)
        {
            if (_bubbles.TryGetValue(index, out var bubble))
            {
                _bubblesConfig.SetBubbleColor(bubble, data);
            }
            else
            {
                Debug.LogWarning($"Bubble at index {index} not found.");
            }
        }
        
        protected void RemoveBubble(Vector2Int index)
        {
            if (_bubbles.TryGetValue(index, out var bubble))
            {
                // TODO: play bubble destroy animation
                Object.Destroy(bubble.gameObject);
                _bubbles.Remove(index);
            }
        }
        
        protected Transform GetBubbleTransform(Vector2Int index)
        {
            return _bubbles.TryGetValue(index, out var bubble) ? bubble.transform : null;
        }
    }
}