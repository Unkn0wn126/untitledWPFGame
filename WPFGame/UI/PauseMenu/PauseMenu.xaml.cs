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
using WPFGame.UI.MainMenu.LoadSaveSubMenu;
using WPFGame.UI.MainMenu.SaveSaveSubMenu;
using WPFGame.UI.MainMenu.SettingsSubMenu;
using WPFGame.UI.MainMenu.SettingsSubMenu.Controls;
using WPFGame.UI.MainMenu.SettingsSubMenu.Graphics;
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
        private GraphicsMenu _graphicsMenu;
        private ControlsMenu _controlsMenu;
        private LoadSaveMenu _loadSaveMenu;
        private SaveSaveMenu _saveSaveMenu;
        public PauseMenu(ProcessMenuButtonClick resumeAction, ProcessMenuButtonClick exitToMainAction, ProcessMenuButtonClick quitGameAction, ProcessSettingsApplyButtonClick settingsApplyAction, Configuration originalConfiguration, ProcessSaveActionButtonClick loadSaveAction, ProcessSaveActionButtonClick saveSaveAction, Uri saveFolder)
        {
            InitializeComponent();
            _loadSaveMenu = new LoadSaveMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), new ProcessSaveActionButtonClick(loadSaveAction), saveFolder);
            _saveSaveMenu = new SaveSaveMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), new ProcessSaveActionButtonClick(saveSaveAction), saveFolder);
            _graphicsMenu = new GraphicsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), settingsApplyAction, originalConfiguration);
            _controlsMenu = new ControlsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), settingsApplyAction, originalConfiguration);
            _settingsMenu = new SettingsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), new ProcessMenuButtonClick(LoadGraphicsMenu), new ProcessMenuButtonClick(LoadControlsMenu));
            InitializeDefault(resumeAction, exitToMainAction, quitGameAction);
        }

        public void RestoreDefaultState()
        {
            MainGrid.Children.RemoveRange(0, MainGrid.Children.Count);
            LoadDefault();
        }

        private void InitializeDefault(ProcessMenuButtonClick resumeAction, ProcessMenuButtonClick exitToMainAction, ProcessMenuButtonClick quitGameAction)
        {
            _defaultMenu = new DefaultPauseMenu(resumeAction, exitToMainAction, quitGameAction, new ProcessMenuButtonClick(LoadSettingsMenu), new ProcessMenuButtonClick(LoadLoadGameMenu), new ProcessMenuButtonClick(LoadSaveGameMenu));
            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }

        private void LoadDefault()
        {
            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }


        private void LoadLoadGameMenu()
        {
            MainGrid.Children.Remove(_defaultMenu);
            MainGrid.Children.Add(_loadSaveMenu);
            _loadSaveMenu.SetValue(Grid.RowProperty, 1);
            _loadSaveMenu.SetValue(Grid.ColumnProperty, 1);
            _loadSaveMenu.UpdateSaveList();
        }

        private void LoadSaveGameMenu()
        {
            MainGrid.Children.Remove(_defaultMenu);
            MainGrid.Children.Add(_saveSaveMenu);
            _saveSaveMenu.SetValue(Grid.RowProperty, 1);
            _saveSaveMenu.SetValue(Grid.ColumnProperty, 1);
            _saveSaveMenu.UpdateSaveList();
        }


        private void LoadControlsMenu()
        {
            MainGrid.Children.Remove(_settingsMenu);
            MainGrid.Children.Add(_controlsMenu);
            _controlsMenu.SetValue(Grid.RowProperty, 0);
            _controlsMenu.SetValue(Grid.RowSpanProperty, 3);
            _controlsMenu.SetValue(Grid.ColumnProperty, 0);
            _controlsMenu.SetValue(Grid.ColumnSpanProperty, 3);
        }

        private void LoadGraphicsMenu()
        {
            MainGrid.Children.Remove(_settingsMenu);
            MainGrid.Children.Add(_graphicsMenu);
            _graphicsMenu.SetValue(Grid.RowProperty, 0);
            _graphicsMenu.SetValue(Grid.RowSpanProperty, 3);
            _graphicsMenu.SetValue(Grid.ColumnProperty, 0);
            _graphicsMenu.SetValue(Grid.ColumnSpanProperty, 3);
        }

        private void LoadSettingsMenu()
        {
            MainGrid.Children.Remove(_defaultMenu);
            MainGrid.Children.Add(_settingsMenu);
            _settingsMenu.SetValue(Grid.RowProperty, 1);
            _settingsMenu.SetValue(Grid.ColumnProperty, 1);
        }

        private void LoadPreviousMenu(UserControl userControl)
        {
            MainGrid.Children.Remove(userControl);
            UserControl parent;
            if (userControl.Equals(_controlsMenu) || userControl.Equals(_graphicsMenu))
            {
                parent = _settingsMenu;
            }
            else
            {
                parent = _defaultMenu;
            }
            MainGrid.Children.Add(parent);
            parent.SetValue(Grid.RowProperty, 1);
            parent.SetValue(Grid.ColumnProperty, 1);
        }
    }
}
