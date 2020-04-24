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
        public DefaultMenu(ProcessMenuButtonClick processSettingsClick, ProcessMenuButtonClick processQuitClick, ProcessMenuButtonClick processNewGameClick)
        {
            InitializeComponent();
            _processSettingsClick = processSettingsClick;
            _processQuitClick = processQuitClick;
            _processNewGameClick = processNewGameClick;
        }

        private void OnNewGameClick(object sender, RoutedEventArgs e)
        {
            _processNewGameClick.Invoke();
        }

        private void OnLoadGameClick(object sender, RoutedEventArgs e)
        {

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
