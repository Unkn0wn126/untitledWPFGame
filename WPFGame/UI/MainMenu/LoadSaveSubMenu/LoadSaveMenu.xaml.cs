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

namespace WPFGame.UI.MainMenu.LoadSaveSubMenu
{
    /// <summary>
    /// Interaction logic for LoadSaveMenu.xaml
    /// </summary>
    public partial class LoadSaveMenu : UserControl
    {
        private ProcessMenuBackButtonClick _backButtonAction;
        public LoadSaveMenu(ProcessMenuBackButtonClick backButtonAction)
        {
            InitializeComponent();
            _backButtonAction = backButtonAction;
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            _backButtonAction.Invoke(this);
        }
    }
}
