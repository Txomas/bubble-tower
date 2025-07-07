using System;
using System.Collections.Generic;
using Game.Input;
using UnityEngine;
using Zenject;
using Zenject.Helpers;
using UnityEditor;

namespace Game
{
    public class LevelEditorController : BaseController
    {
        [Inject] private readonly LevelGridConfig _gridConfig;
        [Inject] private readonly LevelEditorView _view;
        [Inject] private readonly Camera _gameplayCamera;
        [Inject] private readonly LevelModel _levelModel;
        [Inject] private readonly LevelEditorModel _editorModel;
        [Inject] private readonly IGridService _gridService;
        [Inject] private readonly LevelDataService _levelDataManager;
        
        private bool _isPainting;
        private Vector2Int _lastPaintedCell = new(-1, -1);
        private InputSystemActions _actions;

        protected override void OnInitialized()
        {
            _view.SetColorOptions(Enum.GetNames(typeof(BubbleColor)));
            _view.SetViewModeOptions(Enum.GetNames(typeof(LevelViewMode)));
            
            _view.ViewModeChanged.AddListener(mode =>
            {
                _editorModel.SetViewMode((LevelViewMode)mode);
            });
            
            _view.RegenerateButtonClicked.AddListener(RegenerateGrid);
            _view.SaveButtonClicked.AddListener(SaveLevel);
            _view.LoadButtonClicked.AddListener(LoadLevel);
            
            Subscribe<PointerMovedSignal>(OnPointerMoved);
            RegenerateGrid();
        }

        private void OnPointerMoved(PointerMovedSignal signal)
        {
            var ray = _gameplayCamera.ScreenPointToRay(signal.ScreenPosition);
            
            if (Physics.Raycast(ray, out var hit) && hit.collider.TryGetComponent(out BubbleView view))
            {
                if (view.GridIndex != _lastPaintedCell)
                {
                    var index = view.GridIndex;
                    _levelModel.ChangeBubbleColor(index, (BubbleColor)_view.SelectedColor);
                    _lastPaintedCell = index;
                }
            }
        }

        private void RegenerateGrid()
        {
            _levelModel.SetData(new Dictionary<Vector2Int, BubbleColor>());
        }

        private void SaveLevel()
        {
            var unconnected = _gridService.GetUnconnectedCells();
            
            if (unconnected.Count > 0)
            {
                EditorUtility.DisplayDialog("Invalid Level",
                    $"There are {unconnected.Count} floating bubbles. Please fix before saving.", "OK");
                return;
            }
            
            var data = ScriptableObject.CreateInstance<LevelData>();
            data.SetColoredCells(_levelModel.Bubbles);
            var fileName = _levelDataManager.SaveLevelData(data);
            
            EditorUtility.DisplayDialog("Success", $"Level saved as: {fileName}", "OK");
        }

        private void LoadLevel()
        {
            var levelNames = _levelDataManager.GetAvailableLevelNames();
            
            if (levelNames.Count == 0)
            {
                EditorUtility.DisplayDialog("No Levels", $"No saved levels found in {ConfigsPaths.Levels}.", "OK");
                return;
            }
            
            var menu = new GenericMenu();

            foreach (var levelName in levelNames)
            {
                menu.AddItem(new GUIContent(levelName), false, () => LoadSelectedLevel(levelName));
            }
            
            menu.ShowAsContext();
        }

        private void LoadSelectedLevel(string levelName)
        {
        #if UNITY_EDITOR
            var loadedLevelData = _levelDataManager.LoadLevelData(levelName);
            
            if (loadedLevelData != null)
            {
                _levelModel.SetData(loadedLevelData);
                EditorUtility.DisplayDialog("Success", $"Level loaded: {levelName}", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", $"Failed to load level: {levelName}", "OK");
            }
        #endif
        }
    }
}