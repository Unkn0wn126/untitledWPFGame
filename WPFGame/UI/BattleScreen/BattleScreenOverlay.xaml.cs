using Engine.Models.Scenes;
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
        private BattleLowerPartOverlay _lowerPart;
        private LifeStatsOverlay _playerLifeStats;
        private LifeStatsOverlay _enemyLifeStats;
        private CommandConsoleOverlay _commandConsole;
        private ImageResourceManager _imageResourceManager;

        private BattleSceneMediator _battleSceneMediator;
        public BattleScreenOverlay(BattleSceneMediator battleSceneMediator, ImageResourceManager imageResourceManager)
        {
            InitializeComponent();

            InitializeLowerPart();
            InitializePlayerLifeStats();
            InitializeEnemyLifeStats();
            InitializeCommandConsole();

            _imageResourceManager = imageResourceManager;

            _battleSceneMediator = battleSceneMediator;
            _battleSceneMediator.MessageProcessor += UpdateLogMessages;

            UpdateAvatars();
        }

        public void UpdateState()
        {
            UpdateAvatars();
        }

        public void ClearMessageLog()
        {
            _lowerPart.LogTextBox.Text = string.Empty;
        }

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

        private void UpdateLogMessages(string message)
        {
            Dispatcher.Invoke(() => 
            {
                _lowerPart.UpdateLogTextBox(message);
            });
        }

        private void InitializeLowerPart()
        {
            _lowerPart = new BattleLowerPartOverlay();
            MainGrid.Children.Add(_lowerPart);
            _lowerPart.SetValue(Grid.RowProperty, 2);
            _lowerPart.SetValue(Grid.ColumnProperty, 0);
            _lowerPart.SetValue(Grid.ColumnSpanProperty, 3);
        }

        private void InitializePlayerLifeStats()
        {
            _playerLifeStats = new LifeStatsOverlay();
            MainGrid.Children.Add(_playerLifeStats);
            _playerLifeStats.SetValue(Grid.RowProperty, 1);
            _playerLifeStats.SetValue(Grid.ColumnProperty, 0);
        }

        private void InitializeEnemyLifeStats()
        {
            _enemyLifeStats = new LifeStatsOverlay();
            MainGrid.Children.Add(_enemyLifeStats);
            _enemyLifeStats.SetValue(Grid.RowProperty, 1);
            _enemyLifeStats.SetValue(Grid.ColumnProperty, 2);
        }

        private void InitializeCommandConsole()
        {
            _commandConsole = new CommandConsoleOverlay();
            MainGrid.Children.Add(_commandConsole);
            _commandConsole.SetValue(Grid.RowProperty, 1);
            _commandConsole.SetValue(Grid.ColumnProperty, 1);
        }
    }
}
