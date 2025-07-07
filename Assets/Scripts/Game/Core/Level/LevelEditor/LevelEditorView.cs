using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
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
    }
}