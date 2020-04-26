using System;
using System.Windows.Controls;
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

        private Configuration _currentConfig;
        public PauseMenu(ProcessMenuButtonClick resumeAction, ProcessMenuButtonClick exitToMainAction, 
            ProcessMenuButtonClick quitGameAction, ProcessSettingsApplyButtonClick settingsApplyAction, 
            Configuration originalConfiguration, ProcessSaveActionButtonClick loadSaveAction, 
            ProcessSaveActionButtonClick saveSaveAction, Uri saveFolder)
        {
            InitializeComponent();
            _currentConfig = originalConfiguration;

            _loadSaveMenu = new LoadSaveMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), 
                new ProcessSaveActionButtonClick(loadSaveAction), saveFolder);

            _saveSaveMenu = new SaveSaveMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), 
                new ProcessSaveActionButtonClick(saveSaveAction), saveFolder);

            _graphicsMenu = new GraphicsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), 
                settingsApplyAction, originalConfiguration);

            _controlsMenu = new ControlsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), 
                settingsApplyAction, originalConfiguration);

            _settingsMenu = new SettingsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), 
                new ProcessMenuButtonClick(LoadGraphicsMenu), new ProcessMenuButtonClick(LoadControlsMenu));

            InitializeDefault(resumeAction, exitToMainAction, quitGameAction);
        }

        /// <summary>
        /// Updates the game configuration
        /// context for this menu
        /// </summary>
        /// <param name="originalConfiguration"></param>
        public void UpdateConfig(Configuration originalConfiguration)
        {
            _currentConfig = originalConfiguration;
        }

        /// <summary>
        /// Restores the initial state
        /// of this menu
        /// </summary>
        public void RestoreDefaultState()
        {
            MainGrid.Children.RemoveRange(0, MainGrid.Children.Count);
            LoadDefault();
        }

        /// <summary>
        /// Initializes the default menu
        /// for this section
        /// </summary>
        /// <param name="resumeAction"></param>
        /// <param name="exitToMainAction"></param>
        /// <param name="quitGameAction"></param>
        private void InitializeDefault(ProcessMenuButtonClick resumeAction, ProcessMenuButtonClick exitToMainAction, ProcessMenuButtonClick quitGameAction)
        {
            _defaultMenu = new DefaultPauseMenu(resumeAction, exitToMainAction, quitGameAction, 
                new ProcessMenuButtonClick(LoadSettingsMenu), new ProcessMenuButtonClick(LoadLoadGameMenu), 
                new ProcessMenuButtonClick(LoadSaveGameMenu));

            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }

        /// <summary>
        /// Loads the default menu
        /// of this section
        /// </summary>
        private void LoadDefault()
        {
            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }

        /// <summary>
        /// Loads the load game menu
        /// </summary>
        private void LoadLoadGameMenu()
        {
            MainGrid.Children.Remove(_defaultMenu);
            MainGrid.Children.Add(_loadSaveMenu);
            _loadSaveMenu.SetValue(Grid.RowProperty, 1);
            _loadSaveMenu.SetValue(Grid.ColumnProperty, 1);
            _loadSaveMenu.UpdateSaveList();
        }

        /// <summary>
        /// Loads the save game menu
        /// </summary>
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
            _controlsMenu.UpdateOriginalConfiguration(_currentConfig);
            MainGrid.Children.Remove(_settingsMenu);
            MainGrid.Children.Add(_controlsMenu);
            _controlsMenu.SetValue(Grid.RowProperty, 0);
            _controlsMenu.SetValue(Grid.RowSpanProperty, 3);
            _controlsMenu.SetValue(Grid.ColumnProperty, 0);
            _controlsMenu.SetValue(Grid.ColumnSpanProperty, 3);
        }

        private void LoadGraphicsMenu()
        {
            _graphicsMenu.UpdateOriginalConfiguration(_currentConfig);
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
