using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            
            var anyName = BubbleColor.Any.ToString();
            _view.SetColorOptions(Enum.GetNames(typeof(BubbleColor)).Where(name => name != anyName));
            _view.SetViewModeOptions(Enum.GetNames(typeof(LevelViewMode)));
            
            _view.ViewModeChanged.AddListener(OnViewModeChanged);

#if UNITY_EDITOR
            _view.RegenerateButtonClicked.AddListener(RegenerateGrid);
            _view.SaveButtonClicked.AddListener(SaveLevel);
            _view.LoadButtonClicked.AddListener(LoadLevel);
            
            OnViewModeChanged(0);
            RegenerateGrid();
#endif
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

#if UNITY_EDITOR
        private void RegenerateGrid()
        {
            if (ConfirmReset())
            {
                _currentLevelName = string.Empty;
                _model.SetData(new Dictionary<Vector2Int, BubbleData>());
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
            
            if (string.IsNullOrEmpty(_currentLevelName))
            {
                _currentLevelName = LevelDataUtil.SaveNewLevelData(_model.Bubbles);
                EditorUtility.DisplayDialog("Success", $"Level saved as: {_currentLevelName}", "OK");
            }
            else
            {
                LevelDataUtil.SaveLevelData(_currentLevelName, _model.Bubbles);
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
            var loadedLevelData = LevelDataUtil.LoadLevelData(levelName);
            
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
                    return true;
                default:
                    return false;
            }
        }
#endif
    }
}