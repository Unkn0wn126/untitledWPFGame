using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public ControlsMenu(ProcessMenuBackButtonClick cancelAction, ProcessSettingsApplyButtonClick applyAction, Configuration originalConfiguration)
        {
            InitializeComponent();
            _cancelAction = cancelAction;
            _applyAction = applyAction;
            _currentConfiguration = new Configuration(originalConfiguration);
        }

        public void UpdateOriginalConfiguration(Configuration originalConfiguration)
        {
            _currentConfiguration = new Configuration(originalConfiguration);
        }

        private void OnApplyClick(object sender, RoutedEventArgs e)
        {
            _applyAction.Invoke(_currentConfiguration);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            _cancelAction.Invoke(this);
        }
    }
}
