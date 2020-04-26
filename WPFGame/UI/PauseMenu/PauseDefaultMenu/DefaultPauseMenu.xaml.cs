using System.Windows;
using System.Windows.Controls;
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.PauseMenu.PauseDefaultMenu
{
    /// <summary>
    /// Interaction logic for DefaultPauseMenu.xaml
    /// </summary>
    public partial class DefaultPauseMenu : UserControl
    {
        private ProcessMenuButtonClick _resumeAction;
        private ProcessMenuButtonClick _exitToMainAction;
        private ProcessMenuButtonClick _loadGameAction;
        private ProcessMenuButtonClick _saveGameAction;
        private ProcessMenuButtonClick _settingsAction;
        private ProcessMenuButtonClick _quitGameAction;
        public DefaultPauseMenu(ProcessMenuButtonClick resumeAction, ProcessMenuButtonClick exitToMainAction, 
            ProcessMenuButtonClick quitGameAction, ProcessMenuButtonClick settingsAction, 
            ProcessMenuButtonClick loadGameAction, ProcessMenuButtonClick saveGameAction)
        {
            InitializeComponent();
            _resumeAction = resumeAction;
            _exitToMainAction = exitToMainAction;
            _quitGameAction = quitGameAction;
            _settingsAction = settingsAction;
            _loadGameAction = loadGameAction;
            _saveGameAction = saveGameAction;
        }

        private void OnResumeClick(object sender, RoutedEventArgs e)
        {
            _resumeAction.Invoke();
        }

        private void OnExitToMainClick(object sender, RoutedEventArgs e)
        {
            _exitToMainAction.Invoke();
        }

        private void OnLoadGamesClick(object sender, RoutedEventArgs e)
        {
            _loadGameAction.Invoke();
        }

        private void OnSaveGameClick(object sender, RoutedEventArgs e)
        {
            _saveGameAction.Invoke();
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            _settingsAction.Invoke();
        }

        private void OnQuitGameClick(object sender, RoutedEventArgs e)
        {
            _quitGameAction.Invoke();
        }
    }
}
