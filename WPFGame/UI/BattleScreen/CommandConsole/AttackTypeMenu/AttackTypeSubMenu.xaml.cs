using System.Windows;
using System.Windows.Controls;
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.BattleScreen.CommandConsole.AttackTypeMenu
{
    /// <summary>
    /// Interaction logic for AttackTypeSubMenu.xaml
    /// </summary>
    public partial class AttackTypeSubMenu : UserControl
    {
        private readonly ProcessMenuButtonClick _lightAttackAction;
        private readonly ProcessMenuButtonClick _heavyAttackAction;
        private readonly ProcessMenuBackButtonClick _cancelAction;

        public AttackTypeSubMenu(ProcessMenuButtonClick lightAttackAction, 
            ProcessMenuButtonClick heavyAttackAction, ProcessMenuBackButtonClick cancelAction)
        {
            InitializeComponent();
            _lightAttackAction = lightAttackAction;
            _heavyAttackAction = heavyAttackAction;
            _cancelAction = cancelAction;
        }

        private void OnLightAttackButtonClick(object sender, RoutedEventArgs e)
        {
            _lightAttackAction.Invoke();
        }

        private void OnHeavyAttackButtonClick(object sender, RoutedEventArgs e)
        {
            _heavyAttackAction.Invoke();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            _cancelAction.Invoke(this);
        }
    }
}
