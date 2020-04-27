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

namespace WPFGame.UI.BattleScreen.LowerPart
{
    /// <summary>
    /// Interaction logic for BattleLowerPartOverlay.xaml
    /// </summary>
    public partial class BattleLowerPartOverlay : UserControl
    {
        public BattleLowerPartOverlay()
        {
            InitializeComponent();
        }

        public void UpdateLogTextBox(string message)
        {
            LogTextBox.Text += "\n" + message;
        }
    }
}
