using System;
using System.Collections.Generic;
using System.IO;
using Game.Core.Bubbles;
using UnityEditor;
using UnityEngine;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorController : BaseController
    {
        [Inject] private readonly LevelEditorView _view;
        [Inject] private readonly LevelModel _levelModel;
        [Inject] private readonly LevelEditorModel _editorModel;
        [Inject] private readonly IGridService _gridService;
        [Inject] private readonly LevelDataService _levelDataManager;
        private readonly Dictionary<LevelViewMode, BaseController> _controllersByMode = new();
        private string _currentLevelName;

        protected override void OnInitialized()
        {
            CreateController<LevelEditorGridController>();

            // TODO: rewrite with mediator pattern
            _controllersByMode.Add(LevelViewMode.Editor, CreateController<LevelEditorPaintingController>());
            _controllersByMode.Add(LevelViewMode.Preview, CreateController<TowerController>());
            
            _view.SetColorOptions(Enum.GetNames(typeof(BubbleColor)));
            _view.SetViewModeOptions(Enum.GetNames(typeof(LevelViewMode)));
            
            _view.ViewModeChanged.AddListener(OnViewModeChanged);
            
            _view.RegenerateButtonClicked.AddListener(RegenerateGrid);
            _view.SaveButtonClicked.AddListener(SaveLevel);
            _view.LoadButtonClicked.AddListener(LoadLevel);
            
            OnViewModeChanged(0);
            RegenerateGrid();
        }

        private void OnViewModeChanged(int value)
        {
            var mode = (LevelViewMode)value;
            
            foreach (var (key, controller) in _controllersByMode)
            {
                controller.IsEnabled = key == mode;
            }
            
            _editorModel.SetViewMode(mode);
        }

        private void RegenerateGrid()
        {
            if (ConfirmReset())
            {
                _levelModel.SetData(new Dictionary<Vector2Int, BubbleColor>());
            }
        }

        private void SaveLevel()
        {
            var unconnected = _gridService.GetUnconnectedCells();
            
            if (unconnected.Count > 0)
            {
                var indexes = string.Join(", ", unconnected);
                EditorUtility.DisplayDialog("Invalid Level",
                    $"Fix floating {unconnected.Count} bubbles: {indexes}", "OK");
                return;
            }
            
            var data = ScriptableObject.CreateInstance<LevelData>();
            data.SetColoredCells(_levelModel.Bubbles);

            if (string.IsNullOrEmpty(_currentLevelName))
            {
                _currentLevelName = _levelDataManager.SaveLevelData(data);
                EditorUtility.DisplayDialog("Success", $"Level saved as: {_currentLevelName}", "OK");
            }
            else
            {
                _levelDataManager.SaveLevelData(_currentLevelName, data);
                EditorUtility.DisplayDialog("Success", $"Level updated: {_currentLevelName}", "OK");
            }
        }

        private void LoadLevel()
        {
            if (!ConfirmReset())
            {
                return;
            }
            
            var levelPath = EditorUtility.OpenFilePanelWithFilters("Load Level", ConfigsPaths.LevelsFolder, new[] { "Configs", ConfigsPaths.AssetExtension.Replace(".", "") });
            
            if (string.IsNullOrEmpty(levelPath))
            {
                return;
            }
            
            var levelName = Path.GetFileNameWithoutExtension(levelPath);
            var loadedLevelData = _levelDataManager.LoadLevelData(levelName);
            
            if (loadedLevelData != null)
            {
                _levelModel.SetData(loadedLevelData);
                _currentLevelName = levelName;
                EditorUtility.DisplayDialog("Success", $"Level loaded: {levelName}", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", $"Failed to load level: {levelName}", "OK");
            }
        }

        private bool ConfirmReset()
        {
            if (_levelModel.Bubbles.Count <= 0)
            {
                return true;
            }

            var result = EditorUtility.DisplayDialogComplex("Save suggestion",
                "Do you want to save before reset?",
                "Yes", "No", "Cancel");
                
            switch (result)
            {
                case 0: // Save
                    SaveLevel();
                    return true;
                case 1: // Discard
                    _levelModel.SetData(new Dictionary<Vector2Int, BubbleColor>());
                    return true;
                default:
                    return false;
            }
        }
    }
}