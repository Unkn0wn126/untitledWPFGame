using System.Windows;
using System.Windows.Controls;

namespace WPFGame.UI.MainMenu.DefaultView
{
    /// <summary>
    /// Interaction logic for DefaultMenu.xaml
    /// </summary>
    public partial class DefaultMenu : UserControl
    {
        private ProcessMenuButtonClick _processSettingsClick;
        private ProcessMenuButtonClick _processQuitClick;
        private ProcessMenuButtonClick _processNewGameClick;
        private ProcessMenuButtonClick _loadGameAction;
        public DefaultMenu(ProcessMenuButtonClick processSettingsClick, ProcessMenuButtonClick processQuitClick, ProcessMenuButtonClick processNewGameClick, ProcessMenuButtonClick loadGameAction)
        {
            InitializeComponent();
            _processSettingsClick = processSettingsClick;
            _processQuitClick = processQuitClick;
            _processNewGameClick = processNewGameClick;
            _loadGameAction = loadGameAction;
        }

        private void OnNewGameClick(object sender, RoutedEventArgs e)
        {
            _processNewGameClick.Invoke();
        }

        private void OnLoadGameClick(object sender, RoutedEventArgs e)
        {
            _loadGameAction.Invoke();
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            _processSettingsClick.Invoke();
        }

        private void OnQuitGameClick(object sender, RoutedEventArgs e)
        {
            _processQuitClick.Invoke();
        }
    }
}
