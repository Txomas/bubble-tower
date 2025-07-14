using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorView : MonoBehaviour
    {
        [SerializeField] private Button _regenerateButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private TMP_Dropdown _colorDropdown;
        [SerializeField] private TMP_Dropdown _viewModeDropdown;
        
        public Button.ButtonClickedEvent RegenerateButtonClicked => _regenerateButton.onClick;
        public Button.ButtonClickedEvent SaveButtonClicked => _saveButton.onClick;
        public Button.ButtonClickedEvent LoadButtonClicked => _loadButton.onClick;
        public TMP_Dropdown.DropdownEvent ViewModeChanged => _viewModeDropdown.onValueChanged;
        
        public int ColorsOptionsCount => _colorDropdown.options.Count;
        public int SelectedColor => _colorDropdown.value;
        
        public void SetColorOptions(IEnumerable<string> options)
        {
            _colorDropdown.ClearOptions();
            _colorDropdown.AddOptions(options.ToList());
        }
        
        public void SetViewModeOptions(IEnumerable<string> options)
        {
            _viewModeDropdown.ClearOptions();
            _viewModeDropdown.AddOptions(options.ToList());
        }
        
        public void SetSelectedColor(int index)
        {
            var count = _colorDropdown.options.Count;
            index = index < 0 ? count - -index % count : index % count;
            
            _colorDropdown.value = index;
            _colorDropdown.RefreshShownValue();
        }
    }
}