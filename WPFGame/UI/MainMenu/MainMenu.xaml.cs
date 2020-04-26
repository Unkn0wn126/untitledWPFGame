using System;
using System.Windows.Controls;
using WPFGame.UI.MainMenu.DefaultView;
using WPFGame.UI.MainMenu.LoadSaveSubMenu;
using WPFGame.UI.MainMenu.SettingsSubMenu;
using WPFGame.UI.MainMenu.SettingsSubMenu.Controls;
using WPFGame.UI.MainMenu.SettingsSubMenu.Graphics;

namespace WPFGame.UI.MainMenu
{
    public delegate void ProcessMenuButtonClick();
    public delegate void ProcessMenuBackButtonClick(UserControl userControl);
    public delegate void ProcessSettingsApplyButtonClick(Configuration configuration);
    public delegate void ProcessSaveActionButtonClick(Uri path);
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        private DefaultMenu _defaultMenu;
        private SettingsMenu _settingsMenu;
        private GraphicsMenu _graphicsMenu;
        private ControlsMenu _controlsMenu;
        private LoadSaveMenu _loadGameMenu;

        private ProcessMenuButtonClick _quitAction;

        private Configuration _currentConfig;

        public MainMenu(ProcessMenuButtonClick quitAction, ProcessMenuButtonClick newGameAction, ProcessSettingsApplyButtonClick settingsApplyAction, Configuration originalConfiguration, ProcessSaveActionButtonClick loadSaveAction, Uri saveFolder)
        {
            InitializeComponent();
            _quitAction = quitAction;
            _currentConfig = originalConfiguration;
            _graphicsMenu = new GraphicsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), settingsApplyAction, originalConfiguration);
            _controlsMenu = new ControlsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), settingsApplyAction, originalConfiguration);
            _settingsMenu = new SettingsMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), new ProcessMenuButtonClick(LoadGraphicsMenu), new ProcessMenuButtonClick(LoadControlsMenu));
            _loadGameMenu = new LoadSaveMenu(new ProcessMenuBackButtonClick(LoadPreviousMenu), new ProcessSaveActionButtonClick(loadSaveAction), saveFolder);
            _defaultMenu = new DefaultMenu(new ProcessMenuButtonClick(LoadSettingsMenu), _quitAction, newGameAction, new ProcessMenuButtonClick(LoadLoadGameMenu));

            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }

        public void RestoreDefaultState()
        {
            MainGrid.Children.RemoveRange(0, MainGrid.Children.Count);
            LoadDefault();
        }

        private void LoadDefault()
        {
            MainGrid.Children.Add(_defaultMenu);
            _defaultMenu.SetValue(Grid.RowProperty, 1);
            _defaultMenu.SetValue(Grid.ColumnProperty, 1);
        }

        public void UpdateConfig(Configuration originalConfiguration)
        {
            _currentConfig = originalConfiguration;
            _controlsMenu.UpdateOriginalConfiguration(_currentConfig);
            _graphicsMenu.UpdateOriginalConfiguration(_currentConfig);
        }

        private void LoadSettingsMenu()
        {
            MainGrid.Children.Remove(_defaultMenu);
            MainGrid.Children.Add(_settingsMenu);
            _settingsMenu.SetValue(Grid.RowProperty, 1);
            _settingsMenu.SetValue(Grid.ColumnProperty, 1);
        }

        private void LoadLoadGameMenu()
        {
            MainGrid.Children.Remove(_defaultMenu);
            MainGrid.Children.Add(_loadGameMenu);
            _loadGameMenu.SetValue(Grid.RowProperty, 1);
            _loadGameMenu.SetValue(Grid.ColumnProperty, 1);
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
