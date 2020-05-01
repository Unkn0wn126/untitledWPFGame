using System.Windows;
using System.Windows.Controls;

namespace WPFGame.UI.MainMenu.SettingsSubMenu
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : UserControl
    {
        private readonly ProcessMenuBackButtonClick _processMenuBackButtonClick;
        private readonly ProcessMenuButtonClick _graphicsButtonAction;
        private readonly ProcessMenuButtonClick _controlstButtonAction;

        public SettingsMenu(ProcessMenuBackButtonClick processMenuBackButtonClick, 
            ProcessMenuButtonClick graphicsButtonAction, ProcessMenuButtonClick controlstButtonAction)
        {
            InitializeComponent();
            _processMenuBackButtonClick = processMenuBackButtonClick;
            _graphicsButtonAction = graphicsButtonAction;
            _controlstButtonAction = controlstButtonAction;
        }

        private void OnGraphicsClick(object sender, RoutedEventArgs e)
        {
            _graphicsButtonAction.Invoke();
        }

        private void OnControlsClick(object sender, RoutedEventArgs e)
        {
            _controlstButtonAction.Invoke();
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            _processMenuBackButtonClick.Invoke(this);
        }
    }
}
