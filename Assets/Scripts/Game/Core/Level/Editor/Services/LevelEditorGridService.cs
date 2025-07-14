using UnityEngine;
using Zenject;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorGridService : GridService
    {
        [Inject] private readonly LevelEditorModel _model;

        public override Vector3 IndexToLocalPos(Vector2Int cellIndexes)
        {
            var col = cellIndexes.x;
            var row = cellIndexes.y;
            
            if (_model.ViewMode == LevelViewMode.Editor)
            {
                var x = col * CellSize + (row % 2 == 1 ? CellSize * 0.5f : 0f);
                var z = row * HeightStep;
                return new Vector3(x, 0f, z);
            }
            else
            {
                return base.IndexToLocalPos(cellIndexes);
            }
        }

        public override Vector2Int WorldToGridIndex(Vector3 worldPos)
        {
            if (_model.ViewMode == LevelViewMode.Editor)
            {
                var localPos = GetLocalPoint(worldPos);
                var row = Mathf.RoundToInt(localPos.z / HeightStep);
                
                var offset = row % 2 == 1 ? CellSize * 0.5f : 0f;
                var col = Mathf.RoundToInt((localPos.x - offset) / CellSize);
                
                return new Vector2Int(col, row);
            }
            else
            {
                return base.WorldToGridIndex(worldPos);
            }
        }
    }
}