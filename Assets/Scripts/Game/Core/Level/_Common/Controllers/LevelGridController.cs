using Game.Core.Bubbles;
using UnityEngine;
using Zenject;
using Zenject.Helpers;
using Object = UnityEngine.Object;

namespace Game.Core.Level
{
    public class LevelGridController : BaseController
    {
        [Inject] private readonly BubblesConfig _bubblesConfig;
        [Inject] private readonly LevelGridConfig _config;
        [Inject] private readonly IGridModel _model;
        [Inject] private readonly IGridService _service;
        [Inject] private readonly LevelGridView _view;
        private BubbleView[,] _bubbles;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Subscribe<NewLevelGridSet>(Rebuild);
            Subscribe<BubbleChanged>(UpdateCell);
        }

        protected void Rebuild()
        {
            if (_bubbles != null)
            {
                foreach (var bubble in _bubbles)
                {
                    if (bubble != null)
                    {
                        Object.Destroy(bubble.gameObject);
                    }
                }
            }

            var rows = _config.Rows;
            var columns = _config.Columns;
            _bubbles = new BubbleView[columns, rows];

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    var indexes = new Vector2Int(c, r);
                    
                    if (ShouldCreateCell(indexes, out var color))
                    {
                        CreateCell(indexes, color);
                    }
                }
            }
        }

        protected virtual bool ShouldCreateCell(Vector2Int indexes, out BubbleColor color)
        {
            return _model.Bubbles.TryGetValue(indexes, out color);
        }

        private void CreateCell(Vector2Int indexes, BubbleColor bubbleColor)
        {
            var position = _service.GetCellPosition(indexes);
            var view = Object.Instantiate(_config.CellPrefab, position, Quaternion.identity, _view.CellsContainer);
            view.Color = GetColor(bubbleColor);
            view.GridIndex = indexes;
            
            _bubbles[indexes.x, indexes.y] = view;
        }

        private void UpdateCell(BubbleChanged changedData)
        {
            var col = changedData.Position.x;
            var row = changedData.Position.y;
            _bubbles[col, row].Color = GetColor(changedData.NewColor);
        }
        
        private Color GetColor(BubbleColor bubbleColor)
        {
            return _bubblesConfig.GetBubbleColor(bubbleColor);
        }
    }
}