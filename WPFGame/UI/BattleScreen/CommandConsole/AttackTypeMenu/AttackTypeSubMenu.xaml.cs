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
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.BattleScreen.CommandConsole.AttackTypeMenu
{
    /// <summary>
    /// Interaction logic for AttackTypeSubMenu.xaml
    /// </summary>
    public partial class AttackTypeSubMenu : UserControl
    {
        private ProcessMenuButtonClick _lightAttackAction;
        private ProcessMenuButtonClick _heavyAttackAction;
        private ProcessMenuBackButtonClick _cancelAction;

        public AttackTypeSubMenu(ProcessMenuButtonClick lightAttackAction, ProcessMenuButtonClick heavyAttackAction, ProcessMenuBackButtonClick cancelAction)
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
