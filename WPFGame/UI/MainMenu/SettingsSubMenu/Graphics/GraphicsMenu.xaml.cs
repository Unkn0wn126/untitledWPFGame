using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WPFGame.UI.MainMenu.SettingsSubMenu.Graphics
{
    /// <summary>
    /// Interaction logic for GraphicsMenu.xaml
    /// </summary>
    public partial class GraphicsMenu : UserControl
    {
        private ProcessMenuBackButtonClick _cancelAction;
        private ProcessSettingsApplyButtonClick _applyAction;

        private Configuration _currentConfiguration;

        private List<ConfigResolution> _resolutions;
        public GraphicsMenu(ProcessMenuBackButtonClick cancelAction, ProcessSettingsApplyButtonClick applyAction, Configuration originalConfiguration)
        {
            InitializeComponent();
            _cancelAction = cancelAction;
            _applyAction = applyAction;
            _resolutions = new List<ConfigResolution>();
            _currentConfiguration = new Configuration(originalConfiguration);
            InitializeResolutionCombobox();
            InitializeCheckBoxes();
            ResolutionsContainOption(originalConfiguration.Resolution);
        }

        public void UpdateOriginalConfiguration(Configuration originalConfiguration)
        {
            _currentConfiguration = new Configuration(originalConfiguration);
            InitializeCheckBoxes();
            ResolutionsContainOption(_currentConfiguration.Resolution);
        }

        private void ResolutionsContainOption(ConfigResolution resolution)
        {
            foreach (var item in _resolutions)
            {
                if (item.Width == resolution.Width && item.Height == resolution.Height)
                {
                    ResolutionCB.SelectedItem = item;
                    return;
                }
            }

            _resolutions.Add(resolution);
            ResolutionCB.ItemsSource = _resolutions;

            ResolutionCB.SelectedItem = resolution;
        }

        private void InitializeResolutionCombobox()
        {
            _resolutions.Add(new ConfigResolution { Width = 640, Height = 480 });
            _resolutions.Add(new ConfigResolution { Width = 800, Height = 600 });
            _resolutions.Add(new ConfigResolution { Width = 1280, Height = 720 });
            _resolutions.Add(new ConfigResolution { Width = 1920, Height = 1080 });
            ResolutionCB.ItemsSource = _resolutions;
        }

        private void InitializeCheckBoxes()
        {
            WindowModeCheckBox.IsChecked = _currentConfiguration.WindowState == 1;
            WindowStyleCheckBox.IsChecked = _currentConfiguration.WindowStyle == 0;
        }

        // TODO: Bind properties of settings with the configuration

        private void OnApplyClick(object sender, RoutedEventArgs e)
        {
            _currentConfiguration.Resolution = ResolutionCB.SelectedItem as ConfigResolution;
            _currentConfiguration.WindowState = WindowModeCheckBox.IsChecked == true ? 1 : 0;
            _currentConfiguration.WindowStyle = WindowStyleCheckBox.IsChecked == true ? 0 : 1;
            _applyAction.Invoke(_currentConfiguration);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            _cancelAction.Invoke(this);
        }
    }
}
