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

namespace WPFGame.UI.MainMenu.SettingsSubMenu
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : UserControl
    {
        private ProcessMenuBackButtonClick _processMenuBackButtonClick;
        public SettingsMenu(ProcessMenuBackButtonClick processMenuBackButtonClick)
        {
            InitializeComponent();
            _processMenuBackButtonClick = processMenuBackButtonClick;
        }

        private void OnGraphicsClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnControlsClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            _processMenuBackButtonClick.Invoke(this);
        }
    }
}
