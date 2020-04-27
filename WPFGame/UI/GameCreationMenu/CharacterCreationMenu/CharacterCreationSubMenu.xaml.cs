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

namespace WPFGame.UI.GameCreationMenu.CharacterCreationMenu
{
    /// <summary>
    /// Interaction logic for CharacterCreationSubMenu.xaml
    /// </summary>
    public partial class CharacterCreationSubMenu : UserControl
    {
        private ProcessMenuButtonClick _backButtonAction;
        private ProcessMenuButtonClick _acceptButtonAction;
        public GameGenerationInfo GameGenerationInfo { get; set; }
        public CharacterCreationSubMenu(GameGenerationInfo gameGenerationInfo, ProcessMenuButtonClick acceptButtonAction, ProcessMenuButtonClick cancelButtonAction)
        {
            InitializeComponent();
            _acceptButtonAction = acceptButtonAction;
            GameGenerationInfo = gameGenerationInfo;
            _backButtonAction = cancelButtonAction;
        }

        private void OnButtonBackClick(object sender, RoutedEventArgs e)
        {
            _backButtonAction.Invoke();
        }

        private void OnButtonAcceptClick(object sender, RoutedEventArgs e)
        {
            _acceptButtonAction.Invoke();
        }
    }
}
