using System.Windows.Controls;

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

        /// <summary>
        /// Adds the given message
        /// to the battle message log
        /// </summary>
        /// <param name="message"></param>
        public void UpdateLogTextBox(string message)
        {
            LogTextBox.Text += message;
            if (message.Length > 0)
            {
                LogTextBox.Text += "\n####################";
            }
            LogTextBox.ScrollToEnd();
        }
    }
}
