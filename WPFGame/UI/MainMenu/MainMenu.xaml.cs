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
using WPFGame.UI.MainMenu.SettingsSubMenu.Graphics;

namespace WPFGame.UI.MainMenu
{
    public delegate void ProcessMenuButtonClick();
    public delegate void ProcessMenuBackButtonClick(UserControl userControl);
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        private DefaultMenu _defaultMenu;
        private SettingsMenu _settingsMenu;
        private GraphicsMenu _graphicsMenu;

        private ProcessMenuButtonClick _quitAction;

        public MainMenu(ProcessMenuButtonClick quitAction, ProcessMenuButtonClick newGameAction)
        {
            InitializeComponent();
            _quitAction = quitAction;
            _graphicsMenu = new GraphicsMenu();
            _settingsMenu = new SettingsMenu(new ProcessMenuBackButtonClick(RestoreDefaultState), new ProcessMenuButtonClick(LoadGraphicsMenu));
            _defaultMenu = new DefaultMenu(new ProcessMenuButtonClick(LoadSettingsMenu), _quitAction, newGameAction);

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

        private void LoadGraphicsMenu()
        {
            MainGrid.Children.Remove(_settingsMenu);
            MainGrid.Children.Add(_graphicsMenu);
            _graphicsMenu.SetValue(Grid.RowProperty, 0);
            _graphicsMenu.SetValue(Grid.RowSpanProperty, 3);
            _graphicsMenu.SetValue(Grid.ColumnProperty, 0);
            _graphicsMenu.SetValue(Grid.ColumnSpanProperty, 3);
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
