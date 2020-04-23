using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPFGame.UI.MainMenu.DefaultView;
using WPFGame.UI.MainMenu.SettingsSubMenu;

namespace WPFGame.UI.MainMenu
{
    public delegate void ProcessMenuButtonClick();
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        private DefaultMenu _defaultMenu;
        private SettingsMenu _settingsMenu;

        private UserControl _currentMenu;

        private ProcessMenuButtonClick _backButtonAction;

        private ProcessMenuButtonClick _quitAction;

        public MainMenu(ProcessMenuButtonClick quitAction)
        {
            InitializeComponent();
            _quitAction = quitAction;
            _settingsMenu = new SettingsMenu();
            _defaultMenu = new DefaultMenu(new ProcessMenuButtonClick(LoadSettingsMenu), _quitAction);

            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);

            _currentMenu = _defaultMenu;
        }

        private void LoadSettingsMenu()
        {
            MainGrid.Children.Remove(_defaultMenu);
            MainGrid.Children.Add(_settingsMenu);
            _settingsMenu.SetValue(Grid.RowProperty, 1);
            _settingsMenu.SetValue(Grid.ColumnProperty, 1);
            _currentMenu = _settingsMenu;

            ActivateBackButton();
        }

        private void RestoreDefaultState()
        {
            MainGrid.Children.Remove(_currentMenu);
            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }

        private void ActivateBackButton()
        {
            BackButton.Visibility = Visibility.Visible;
            _backButtonAction = RestoreDefaultState;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            _backButtonAction.Invoke();
            BackButton.Visibility = Visibility.Hidden;
        }
    }
}
