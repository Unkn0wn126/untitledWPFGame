using System.Windows;
using System.Windows.Controls;
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.GameCreationMenu.GameMapsGenerationMenu
{
    /// <summary>
    /// Interaction logic for GameMapsGenerationSubMenu.xaml
    /// </summary>
    public partial class GameMapsGenerationSubMenu : UserControl
    {
        private ProcessMenuButtonClick _cancelButtonAction;
        private ProcessMenuButtonClick _acceptButtonAction;
        public GameGenerationInfo GameGenerationInfo { get; set; }
        public GameMapsGenerationSubMenu(GameGenerationInfo gameGenerationInfo, ProcessMenuButtonClick acceptButtonAction,  ProcessMenuButtonClick cancelButtonAction)
        {
            InitializeComponent();
            _acceptButtonAction = acceptButtonAction;
            GameGenerationInfo = gameGenerationInfo;
            _cancelButtonAction = cancelButtonAction;
        }

        private void OnButtonAcceptClick(object sender, RoutedEventArgs e)
        {
            GameGenerationInfo.MinOnX = (int)MaximumOnXSlider.Minimum;
            GameGenerationInfo.MaxOnX = (int)MaximumOnXSlider.Value;            
            
            GameGenerationInfo.MinOnY = (int)MaximumOnYSlider.Minimum;
            GameGenerationInfo.MaxOnY = (int)MaximumOnYSlider.Value;

            GameGenerationInfo.NumberOfLevels = (int)MaximumLevelsSlider.Value;

            _acceptButtonAction.Invoke();
        }

        private void OnButtonCancelClick(object sender, RoutedEventArgs e)
        {
            _cancelButtonAction.Invoke();
        }
    }
}
