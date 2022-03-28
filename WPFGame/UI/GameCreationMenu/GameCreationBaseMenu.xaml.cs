using System.Windows.Controls;
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
        private readonly GameMapsGenerationSubMenu _gameMapsGenerationSubMenu;
        private readonly CharacterCreationSubMenu _characterCreationSubMenu;

        private readonly GameCreationFinalizer _gameCreationFinishedAction;

        private GameGenerationInfo _gameGenerationInfo;

        public GameCreationBaseMenu(ProcessMenuButtonClick mapGenerationBackButtonAction, 
            GameCreationFinalizer gameCreationFinishedAction)
        {
            InitializeComponent();
            _gameGenerationInfo = new GameGenerationInfo();
            _gameCreationFinishedAction = gameCreationFinishedAction;
            _gameMapsGenerationSubMenu = new GameMapsGenerationSubMenu(_gameGenerationInfo, 
                new ProcessMenuButtonClick(LoadCharacterCreationMenu), 
                new ProcessMenuButtonClick(mapGenerationBackButtonAction));

            _characterCreationSubMenu = new CharacterCreationSubMenu(_gameGenerationInfo, 
                new ProcessMenuButtonClick(ProcessCharacterCreationFinished), 
                new ProcessMenuButtonClick(RestoreDefaultState));

            RestoreDefaultState();
        }

        /// <summary>
        /// Invokes the action that
        /// is bound to happen when
        /// the character creation
        /// process is finished
        /// </summary>
        private void ProcessCharacterCreationFinished()
        {
            _gameGenerationInfo = _characterCreationSubMenu.GameGenerationInfo;
            _gameCreationFinishedAction.Invoke(_gameGenerationInfo);
        }

        /// <summary>
        /// Loads the character creation menu
        /// </summary>
        private void LoadCharacterCreationMenu()
        {
            _gameGenerationInfo = _gameMapsGenerationSubMenu.GameGenerationInfo;
            RemoveComponent(_gameMapsGenerationSubMenu);
            AddComponent(_characterCreationSubMenu);
            SetUpCharacterCreationMenu();
        }

        /// <summary>
        /// Restores the default state
        /// of this menu section
        /// </summary>
        public void RestoreDefaultState()
        {
            RemoveComponent(_characterCreationSubMenu);
            AddComponent(_gameMapsGenerationSubMenu);
            SetUpGameGenerationMenu();
            _gameGenerationInfo = new GameGenerationInfo();
            _gameMapsGenerationSubMenu.GameGenerationInfo = _gameGenerationInfo;
            _characterCreationSubMenu.GameGenerationInfo = _gameGenerationInfo;
        }

        /// <summary>
        /// Sets the column property
        /// and row property of the
        /// maps generation menu
        /// </summary>
        private void SetUpGameGenerationMenu()
        {
            _gameMapsGenerationSubMenu.SetValue(Grid.ColumnProperty, 1);
            _gameMapsGenerationSubMenu.SetValue(Grid.RowProperty, 1);
        }

        /// <summary>
        /// Sets the column property
        /// and row property of the
        /// character creation menu
        /// </summary>
        private void SetUpCharacterCreationMenu()
        {
            _characterCreationSubMenu.GameGenerationInfo = _gameGenerationInfo;
            _characterCreationSubMenu.SetValue(Grid.ColumnProperty, 1);
            _characterCreationSubMenu.SetValue(Grid.RowProperty, 1);
        }

        /// <summary>
        /// Helper method to add a component
        /// only if not already present
        /// </summary>
        /// <param name="control"></param>
        private void AddComponent(UserControl control)
        {
            if (!MainGrid.Children.Contains(control))
            {
                MainGrid.Children.Add(control);
            }
        }

        /// <summary>
        /// Helper method to remove a component
        /// only if present on the scene
        /// </summary>
        /// <param name="control"></param>
        private void RemoveComponent(UserControl control)
        {
            if (MainGrid.Children.Contains(control))
            {
                MainGrid.Children.Remove(control);
            }
        }
    }
}
