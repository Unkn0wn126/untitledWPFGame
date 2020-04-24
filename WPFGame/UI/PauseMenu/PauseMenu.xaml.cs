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
using WPFGame.UI.MainMenu;
using WPFGame.UI.MainMenu.SettingsSubMenu;
using WPFGame.UI.PauseMenu.PauseDefaultMenu;

namespace WPFGame.UI.PauseMenu
{
    /// <summary>
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        private DefaultPauseMenu _defaultMenu;
        private SettingsMenu _settingsMenu;
        public PauseMenu(ProcessMenuButtonClick resumeAction, ProcessMenuButtonClick exitToMainAction, ProcessMenuButtonClick quitGameAction)
        {
            InitializeComponent();
            _settingsMenu = new SettingsMenu(new ProcessMenuBackButtonClick(RestoreDefaultState));
            InitializeDefault(resumeAction, exitToMainAction, quitGameAction);
        }

        private void InitializeDefault(ProcessMenuButtonClick resumeAction, ProcessMenuButtonClick exitToMainAction, ProcessMenuButtonClick quitGameAction)
        {
            _defaultMenu = new DefaultPauseMenu(resumeAction, exitToMainAction, quitGameAction, new ProcessMenuButtonClick(LoadSettingsMenu));
            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }

        private void LoadSettingsMenu()
        {
            MainGrid.Children.Remove(_defaultMenu);
            MainGrid.Children.Add(_settingsMenu);
            _settingsMenu.SetValue(Grid.RowProperty, 1);
            _settingsMenu.SetValue(Grid.ColumnProperty, 1);
        }

        private void RestoreDefaultState(UserControl userControl)
        {
            MainGrid.Children.Remove(userControl);
            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }
    }
}
