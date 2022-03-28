using System.Windows;
using System.Windows.Controls;
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.BattleScreen.CommandConsole.MovementTypeMenu
{
    /// <summary>
    /// Interaction logic for MovementTypeSubMenu.xaml
    /// </summary>
    public partial class MovementTypeSubMenu : UserControl
    {
        private readonly ProcessMenuButtonClick _passButtonAction;
        private readonly ProcessMenuButtonClick _healButtonAction;
        private readonly ProcessMenuButtonClick _attackButtonAction;

        public MovementTypeSubMenu(ProcessMenuButtonClick passButtonAction, 
            ProcessMenuButtonClick healButtonAction, 
            ProcessMenuButtonClick attackButtonAction)
        {
            InitializeComponent();

            _passButtonAction = passButtonAction;
            _healButtonAction = healButtonAction;
            _attackButtonAction = attackButtonAction;
        }

        private void OnPassButtonClick(object sender, RoutedEventArgs e)
        {
            _passButtonAction.Invoke();
        }

        private void OnAttackButtonClick(object sender, RoutedEventArgs e)
        {
            _attackButtonAction.Invoke();
        }

        private void OnHealButtonClick(object sender, RoutedEventArgs e)
        {
            _healButtonAction.Invoke();
        }
    }
}
