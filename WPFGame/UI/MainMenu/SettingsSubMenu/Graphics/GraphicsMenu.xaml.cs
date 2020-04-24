﻿using System;
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
            _resolutions.Add(new ConfigResolution { Width = 800, Height = 600 });
            _resolutions.Add(new ConfigResolution { Width = 1280, Height = 720 });
            _resolutions.Add(new ConfigResolution { Width = 1920, Height = 1080 });
            ResolutionCB.ItemsSource = _resolutions;
        }

        private void InitializeCheckBoxes()
        {
            if (_currentConfiguration.WindowState == 0)
            {
                WindowModeCheckBox.IsChecked = false;
            }
            if (_currentConfiguration.WindowStyle == 1)
            {
                WindowStyleCheckBox.IsChecked = false;
            }
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
