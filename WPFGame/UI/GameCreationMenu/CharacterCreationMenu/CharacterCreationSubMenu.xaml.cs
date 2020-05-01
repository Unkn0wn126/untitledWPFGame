using Engine.Models.Components.Life;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPFGame.UI.MainMenu;

namespace WPFGame.UI.GameCreationMenu.CharacterCreationMenu
{
    /// <summary>
    /// Interaction logic for CharacterCreationSubMenu.xaml
    /// </summary>
    public partial class CharacterCreationSubMenu : UserControl
    {
        private readonly ProcessMenuButtonClick _backButtonAction;
        private readonly ProcessMenuButtonClick _acceptButtonAction;
        public GameGenerationInfo GameGenerationInfo { get; set; }
        public CharacterCreationSubMenu(GameGenerationInfo gameGenerationInfo, 
            ProcessMenuButtonClick acceptButtonAction, ProcessMenuButtonClick cancelButtonAction)
        {
            InitializeComponent();
            _acceptButtonAction = acceptButtonAction;
            GameGenerationInfo = gameGenerationInfo;
            _backButtonAction = cancelButtonAction;

            InitializeGenderCombobox();
            InitializeRaceCombobox();
            InitializeBattleClassCombobox();
        }

        /// <summary>
        /// Initializes the gender combobox
        /// </summary>
        private void InitializeGenderCombobox()
        {
            List<string> genderValues = new List<string>(Enum.GetNames(typeof(Gender)));
            GenderCB.ItemsSource = genderValues;
            GenderCB.SelectedIndex = 0;
        }

        /// <summary>
        /// Initializes the race combobox
        /// </summary>
        private void InitializeRaceCombobox()
        {
            List<string> raceValues = new List<string>(Enum.GetNames(typeof(Race)));
            RaceCB.ItemsSource = raceValues;
            RaceCB.SelectedIndex = 0;
        }

        /// <summary>
        /// Initializes the battle class combobox
        /// </summary>
        private void InitializeBattleClassCombobox()
        {
            List<string> battleClassValues = new List<string>(Enum.GetNames(typeof(BattleClass)));
            BattleClassCB.ItemsSource = battleClassValues;
            BattleClassCB.SelectedIndex = 0;
        }

        private void OnButtonBackClick(object sender, RoutedEventArgs e)
        {
            _backButtonAction.Invoke();
        }

        private void OnButtonAcceptClick(object sender, RoutedEventArgs e)
        {
            GameGenerationInfo.PlayerName = TextBlockName.Text;
            GameGenerationInfo.PlayerGender = Enum.Parse<Gender>(GenderCB.SelectedItem.ToString());
            GameGenerationInfo.PlayerRace = Enum.Parse<Race>(RaceCB.SelectedItem.ToString());
            GameGenerationInfo.BattleClass = Enum.Parse<BattleClass>(BattleClassCB.SelectedItem.ToString());
            _acceptButtonAction.Invoke();
        }

        private void OnTextBoxNameChanged(object sender, TextChangedEventArgs e)
        {
            if (AcceptButton != null)
            {
                AcceptButton.IsEnabled = TextBlockName.Text.Length > 0;
            }
        }
    }
}
