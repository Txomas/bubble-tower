using System;
using System.Collections.Generic;
using System.IO;
using Common.Extensions;
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
        [Inject] private readonly LevelGridView _gridView;
        [Inject] private readonly LevelEditorModel _model;
        [Inject] private readonly IGridService _gridService;
        [Inject] private readonly LevelDataService _levelDataManager;
        private readonly Dictionary<LevelViewMode, IActivatable> _controllersByMode = new();
        private string _currentLevelName;

        protected override void OnInitialized()
        {
            CreateController<LevelEditorGridController>();

            var modes = Enum.GetValues(typeof(LevelViewMode));
            
            foreach (LevelViewMode mode in modes)
            {
                var controller = CreateControllerWithId<IActivatable>(mode);
                _controllersByMode.Add(mode, controller);
            }
            
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
            
            _gridView.Tower.gameObject.SetActive(mode is LevelViewMode.Preview);
            _model.SetViewMode(mode);
        }

        private void RegenerateGrid()
        {
            if (ConfirmReset())
            {
                _model.SetData(new Dictionary<Vector2Int, BubbleColor>());
            }
        }

        private void SaveLevel()
        {
            var unconnected = _gridService.GetFloatingBubbles();
            
            if (unconnected.Count > 0)
            {
                var indexes = string.Join(", ", unconnected);
                EditorUtility.DisplayDialog("Invalid Level",
                    $"Fix floating {unconnected.Count} bubbles: {indexes}", "OK");
                return;
            }
            
            var data = ScriptableObject.CreateInstance<LevelData>();
            data.SetColoredCells(_model.Bubbles);

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
                _model.SetData(loadedLevelData);
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
            if (_model.Bubbles.Count <= 0)
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
                    _model.SetData(new Dictionary<Vector2Int, BubbleColor>());
                    return true;
                default:
                    return false;
            }
        }
    }
}