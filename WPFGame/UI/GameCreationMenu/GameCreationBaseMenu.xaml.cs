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
using WPFGame.UI.GameCreationMenu.CharacterCreationMenu;
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
        private CharacterCreationSubMenu _characterCreationSubMenu;

        private GameCreationFinalizer _gameCreationFinishedAction;
        private GameGenerationInfo _gameGenerationInfo;

        public GameCreationBaseMenu(ProcessMenuButtonClick mapGenerationBackButtonAction, GameCreationFinalizer gameCreationFinishedAction)
        {
            InitializeComponent();
            _gameGenerationInfo = new GameGenerationInfo();
            _gameCreationFinishedAction = gameCreationFinishedAction;
            _gameMapsGenerationSubMenu = new GameMapsGenerationSubMenu(_gameGenerationInfo, new ProcessMenuButtonClick(LoadCharacterCreationMenu), new ProcessMenuButtonClick(mapGenerationBackButtonAction));
            _characterCreationSubMenu = new CharacterCreationSubMenu(_gameGenerationInfo, new ProcessMenuButtonClick(ProcessCharacterCreationFinished), new ProcessMenuButtonClick(RestoreDefaultState));
            RestoreDefaultState();
        }

        private void ProcessCharacterCreationFinished()
        {
            _gameGenerationInfo = _characterCreationSubMenu.GameGenerationInfo;
            _gameCreationFinishedAction.Invoke(_gameGenerationInfo);
        }

        private void LoadCharacterCreationMenu()
        {
            _gameGenerationInfo = _gameMapsGenerationSubMenu.GameGenerationInfo;
            RemoveComponent(_gameMapsGenerationSubMenu);
            AddComponent(_characterCreationSubMenu);
            SetUpCharacterCreationMenu();
        }

        public void RestoreDefaultState()
        {
            RemoveComponent(_characterCreationSubMenu);
            AddComponent(_gameMapsGenerationSubMenu);
            SetUpGameGenerationMenu();
            _gameGenerationInfo = new GameGenerationInfo();
            _gameMapsGenerationSubMenu.GameGenerationInfo = _gameGenerationInfo;
            _characterCreationSubMenu.GameGenerationInfo = _gameGenerationInfo;
        }

        private void SetUpGameGenerationMenu()
        {
            _gameMapsGenerationSubMenu.SetValue(Grid.ColumnProperty, 1);
            _gameMapsGenerationSubMenu.SetValue(Grid.RowProperty, 1);
        }

        private void SetUpCharacterCreationMenu()
        {
            _characterCreationSubMenu.GameGenerationInfo = _gameGenerationInfo;
            _characterCreationSubMenu.SetValue(Grid.ColumnProperty, 1);
            _characterCreationSubMenu.SetValue(Grid.RowProperty, 1);
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
