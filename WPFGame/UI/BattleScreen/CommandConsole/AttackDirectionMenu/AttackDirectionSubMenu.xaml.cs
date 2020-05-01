using System.Windows;
using System.Windows.Controls;
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.BattleScreen.CommandConsole.AttackDirectionMenu
{
    /// <summary>
    /// Interaction logic for AttackDirectionSubMenu.xaml
    /// </summary>
    public partial class AttackDirectionSubMenu : UserControl
    {
        private readonly ProcessMenuButtonClick _headAction;
        private readonly ProcessMenuButtonClick _leftAction;
        private readonly ProcessMenuButtonClick _rightAction;
        private readonly ProcessMenuButtonClick _bottomAction;
        private readonly ProcessMenuBackButtonClick _cancelAction;

        public AttackDirectionSubMenu(ProcessMenuButtonClick headAction, 
            ProcessMenuButtonClick leftAction, ProcessMenuButtonClick rightAction, 
            ProcessMenuButtonClick bottomAction, ProcessMenuBackButtonClick cancelAction)
        {
            InitializeComponent();

            _headAction = headAction;
            _leftAction = leftAction;
            _rightAction = rightAction;
            _bottomAction = bottomAction;
            _cancelAction = cancelAction;
        }

        private void OnHeadButtonClick(object sender, RoutedEventArgs e)
        {
            _headAction.Invoke();
        }

        private void OnBottomButtonClick(object sender, RoutedEventArgs e)
        {
            _leftAction.Invoke();
        }

        private void OnLeftButtonClick(object sender, RoutedEventArgs e)
        {
            _rightAction.Invoke();
        }

        private void OnRightButtonClick(object sender, RoutedEventArgs e)
        {
            _bottomAction.Invoke();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            _cancelAction.Invoke(this);
        }
    }
}
