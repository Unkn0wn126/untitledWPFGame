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

namespace WPFGame.UI.BattleScreen.CommandConsole.MovementTypeMenu
{
    /// <summary>
    /// Interaction logic for MovementTypeSubMenu.xaml
    /// </summary>
    public partial class MovementTypeSubMenu : UserControl
    {
        private ProcessMenuButtonClick _passButtonAction;
        private ProcessMenuButtonClick _healButtonAction;
        private ProcessMenuButtonClick _attackButtonAction;

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
