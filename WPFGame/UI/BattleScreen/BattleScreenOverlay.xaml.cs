using Engine.Models.Scenes;
using System.Windows.Controls;
using WPFGame.ResourceManagers;
using WPFGame.UI.BattleScreen.CommandConsole;
using WPFGame.UI.BattleScreen.LifeStats;
using WPFGame.UI.BattleScreen.LowerPart;

namespace WPFGame.UI.BattleScreen
{
    /// <summary>
    /// Interaction logic for BattleScreenOverlay.xaml
    /// </summary>
    public partial class BattleScreenOverlay : UserControl
    {
        private readonly ImageResourceManager _imageResourceManager;
        private readonly BattleSceneMediator _battleSceneMediator;

        private BattleLowerPartOverlay _lowerPart;
        private LifeStatsOverlay _playerLifeStats;
        private LifeStatsOverlay _enemyLifeStats;
        private CommandConsoleOverlay _commandConsole;

        public BattleScreenOverlay(BattleSceneMediator battleSceneMediator, 
            ImageResourceManager imageResourceManager)
        {
            InitializeComponent();
            _imageResourceManager = imageResourceManager;

            _battleSceneMediator = battleSceneMediator;
            _battleSceneMediator.MessageProcessor += UpdateLogMessages;

            InitializeLowerPart();
            InitializePlayerLifeStats();
            InitializeEnemyLifeStats();
            InitializeCommandConsole();


            UpdateState();
        }

        /// <summary>
        /// Updates the state of 
        /// the battle screen
        /// </summary>
        public void UpdateState()
        {
            UpdateAvatars();
            UpdateLifeStats();
        }

        /// <summary>
        /// Updates availability state
        /// of certain battle actions
        /// for the player
        /// </summary>
        private void UpdateActionAvailability()
        {
            Dispatcher.Invoke(() =>
            {
                _commandConsole.UpdateButtonAvailability();
            });
        }

        /// <summary>
        /// Updates the life stats of
        /// the player and the enemy
        /// </summary>
        private void UpdateLifeStats()
        {
            Dispatcher.Invoke(() => 
            {
                _playerLifeStats.UpdateStats(_battleSceneMediator.PlayerLife);
                _enemyLifeStats.UpdateStats(_battleSceneMediator.EnemyLife);
            });
        }

        /// <summary>
        /// Clears the battle message log
        /// </summary>
        public void ClearMessageLog()
        {
            _lowerPart.LogTextBox.Text = string.Empty;
        }

        /// <summary>
        /// Updates the battle avatars
        /// </summary>
        private void UpdateAvatars()
        {
            Dispatcher.Invoke(() =>
            {
                if (_battleSceneMediator.PlayerAvatar != null && _battleSceneMediator.EnemyAvatar != null)
                {
                    _lowerPart.PlayerImage.Source = _imageResourceManager.GetImage(_battleSceneMediator.PlayerAvatar.CurrentImageName);
                    _lowerPart.EnemyImage.Source = _imageResourceManager.GetImage(_battleSceneMediator.EnemyAvatar.CurrentImageName);
                }
            });
        }

        /// <summary>
        /// Updates the battle log messages
        /// </summary>
        /// <param name="message"></param>
        private void UpdateLogMessages(string message)
        {
            Dispatcher.Invoke(() => 
            {
                _lowerPart.UpdateLogTextBox(message);
            });

            UpdateLifeStats();
            UpdateActionAvailability();
        }

        /// <summary>
        /// Initializes the lower part
        /// of the battle screen
        /// </summary>
        private void InitializeLowerPart()
        {
            _lowerPart = new BattleLowerPartOverlay();
            MainGrid.Children.Add(_lowerPart);
            _lowerPart.SetValue(Grid.RowProperty, 2);
            _lowerPart.SetValue(Grid.ColumnProperty, 0);
            _lowerPart.SetValue(Grid.ColumnSpanProperty, 3);
        }

        /// <summary>
        /// Initializes the player life stats
        /// </summary>
        private void InitializePlayerLifeStats()
        {
            _playerLifeStats = new LifeStatsOverlay();
            MainGrid.Children.Add(_playerLifeStats);
            _playerLifeStats.SetValue(Grid.RowProperty, 1);
            _playerLifeStats.SetValue(Grid.ColumnProperty, 0);
        }

        /// <summary>
        /// Initializes the enemy life stats
        /// </summary>
        private void InitializeEnemyLifeStats()
        {
            _enemyLifeStats = new LifeStatsOverlay();
            MainGrid.Children.Add(_enemyLifeStats);
            _enemyLifeStats.SetValue(Grid.RowProperty, 1);
            _enemyLifeStats.SetValue(Grid.ColumnProperty, 2);
        }

        /// <summary>
        /// Initializes the battle
        /// command options for the player
        /// </summary>
        private void InitializeCommandConsole()
        {
            _commandConsole = new CommandConsoleOverlay(_battleSceneMediator);
            MainGrid.Children.Add(_commandConsole);
            _commandConsole.SetValue(Grid.RowProperty, 1);
            _commandConsole.SetValue(Grid.ColumnProperty, 1);
        }
    }
}
