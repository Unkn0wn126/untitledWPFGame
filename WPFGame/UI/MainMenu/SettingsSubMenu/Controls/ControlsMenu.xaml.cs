using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFGame.UI.MainMenu.SettingsSubMenu.Controls
{
    /// <summary>
    /// Interaction logic for ControlsMenu.xaml
    /// </summary>
    public partial class ControlsMenu : UserControl
    {
        private ProcessMenuBackButtonClick _cancelAction;
        private ProcessSettingsApplyButtonClick _applyAction;

        private Configuration _currentConfiguration;

        private List<Key> _possibleKeys;

        public ControlsMenu(ProcessMenuBackButtonClick cancelAction, ProcessSettingsApplyButtonClick applyAction, Configuration originalConfiguration)
        {
            InitializeComponent();
            _cancelAction = cancelAction;
            _applyAction = applyAction;
            _possibleKeys = GetPossibleKeyValues();
            InitializeCBOptions();

            UpdateSelectedItems(originalConfiguration);
        }

        private List<Key> GetPossibleKeyValues()
        {
            List<Key> keys = new List<Key>();
            var values = Enum.GetValues(typeof(Key));
            foreach (var item in values)
            {
                keys.Add((Key)item);
            }

            return keys;
        }

        public void UpdateSelectedItems(Configuration originalConfiguration)
        {
            _currentConfiguration = new Configuration(originalConfiguration);
            UpCB.SelectedItem = _currentConfiguration.Up;
            DownCB.SelectedItem = _currentConfiguration.Down;
            LeftCB.SelectedItem = _currentConfiguration.Left;
            RightCB.SelectedItem = _currentConfiguration.Right;
            ActionCB.SelectedItem = _currentConfiguration.Action;
            DetectiveModeCB.SelectedItem = _currentConfiguration.DetectiveMode;
            EscapeCB.SelectedItem = _currentConfiguration.Escape;
            BackCB.SelectedItem = _currentConfiguration.Back;
            SpaceCB.SelectedItem = _currentConfiguration.Space;
        }

        private void InitializeCBOptions()
        {
            UpCB.ItemsSource = _possibleKeys;
            DownCB.ItemsSource = _possibleKeys;
            LeftCB.ItemsSource = _possibleKeys;
            RightCB.ItemsSource = _possibleKeys;
            ActionCB.ItemsSource = _possibleKeys;
            DetectiveModeCB.ItemsSource = _possibleKeys;
            EscapeCB.ItemsSource = _possibleKeys;
            BackCB.ItemsSource = _possibleKeys;
            SpaceCB.ItemsSource = _possibleKeys;
        }

        public void UpdateOriginalConfiguration(Configuration originalConfiguration)
        {
            _currentConfiguration = new Configuration(originalConfiguration);
            UpdateSelectedItems(_currentConfiguration);
        }

        private void OnApplyClick(object sender, RoutedEventArgs e)
        {
            _currentConfiguration.Up = (Key)UpCB.SelectedItem;
            _currentConfiguration.Down = (Key)DownCB.SelectedItem;
            _currentConfiguration.Left = (Key)LeftCB.SelectedItem;
            _currentConfiguration.Right = (Key)RightCB.SelectedItem;
            _currentConfiguration.Action = (Key)ActionCB.SelectedItem;
            _currentConfiguration.DetectiveMode = (Key)DetectiveModeCB.SelectedItem;
            _currentConfiguration.Escape = (Key)EscapeCB.SelectedItem;
            _currentConfiguration.Back = (Key)BackCB.SelectedItem;
            _currentConfiguration.Space = (Key)SpaceCB.SelectedItem;
            _applyAction.Invoke(_currentConfiguration);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            _cancelAction.Invoke(this);
        }
    }
}
