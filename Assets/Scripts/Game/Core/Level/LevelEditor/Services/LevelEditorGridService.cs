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
            var cellSize = _gridConfig.CellSize;
            
            if (_model.ViewMode == LevelViewMode.Editor)
            {
                var x = col * cellSize + (row % 2 == 1 ? cellSize * 0.5f : 0f);
                var z = row * cellSize * 0.75f;
                return new Vector3(x, 0f, z);
            }
            else
            {
                return base.IndexToLocalPos(cellIndexes);
            }
        }
    }
}