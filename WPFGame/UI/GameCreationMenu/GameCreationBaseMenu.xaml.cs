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
using WPFGame.UI.GameCreationMenu.GameMapsGenerationMenu;
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.GameCreationMenu
{
    public delegate void GameCreationFinalizer(GameGenerationInfo gameGenerationInfo);
    /// <summary>
    /// Interaction logic for GameCreationBaseMenu.xaml
    /// </summary>
    public partial class GameCreationBaseMenu : UserControl
    {
        private GameMapsGenerationSubMenu _gameMapsGenerationSubMenu;
        private GameCreationFinalizer _gameCreationFinishedAction;
        private GameGenerationInfo _gameGenerationInfo;

        public GameCreationBaseMenu(ProcessMenuButtonClick mapGenerationBackButtonAction, GameCreationFinalizer gameCreationFinishedAction)
        {
            InitializeComponent();
            _gameGenerationInfo = new GameGenerationInfo();
            _gameCreationFinishedAction = gameCreationFinishedAction;
            _gameMapsGenerationSubMenu = new GameMapsGenerationSubMenu(_gameGenerationInfo, new ProcessMenuButtonClick(LoadCharacterCreationMenu), new ProcessMenuButtonClick(mapGenerationBackButtonAction));

            RestoreDefaultState();
        }

        private void LoadCharacterCreationMenu()
        {
            _gameGenerationInfo = _gameMapsGenerationSubMenu.GameGenerationInfo;
            _gameCreationFinishedAction.Invoke(_gameGenerationInfo);
        }

        public void RestoreDefaultState()
        {
            AddComponent(_gameMapsGenerationSubMenu);
            SetUpGameGenerationMenu();
            _gameGenerationInfo = new GameGenerationInfo();
        }

        private void SetUpGameGenerationMenu()
        {
            _gameMapsGenerationSubMenu.SetValue(Grid.ColumnProperty, 1);
            _gameMapsGenerationSubMenu.SetValue(Grid.RowProperty, 1);
        }

        private void AddComponent(UserControl control)
        {
            if (!MainGrid.Children.Contains(control))
            {
                MainGrid.Children.Add(control);
            }
        }

        private void RemoveComponent(UserControl control)
        {
            if (MainGrid.Children.Contains(control))
            {
                MainGrid.Children.Remove(control);
            }
        }
    }
}
