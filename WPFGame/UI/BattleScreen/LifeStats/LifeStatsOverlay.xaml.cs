using Engine.Models.Components.Life;
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

namespace WPFGame.UI.BattleScreen.LifeStats
{
    /// <summary>
    /// Interaction logic for LifeStatsOverlay.xaml
    /// </summary>
    public partial class LifeStatsOverlay : UserControl
    {
        public LifeStatsOverlay()
        {
            InitializeComponent();
        }

        public void UpdateStats(ILifeComponent entityStats)
        {
            if (entityStats != null)
            {
                TextBlockName.Text = entityStats.Name;
                TextBlockRace.Text = GetNameOfRace(entityStats.Race);
                TextBlockGender.Text = GetNameOfGender(entityStats.Gender);
                TextBlockBattleClass.Text = GetNameOfBattleClass(entityStats.BattleClass);
                TextBlockStrength.Text = entityStats.Strength.ToString();
                TextBlockAgility.Text = entityStats.Agility.ToString();
                TextBlockIntelligence.Text = entityStats.Intelligence.ToString();
                StaminaBar.Value = GetPercentalValue(entityStats.MaxStamina, entityStats.Stamina);
                HPBar.Value = GetPercentalValue(entityStats.MaxHP, entityStats.HP);
                MPBar.Value = GetPercentalValue(entityStats.MaxMP, entityStats.MP);
                TextBlockCurrentLevel.Text = entityStats.CurrentLevel.ToString();
            }
        }

        private float GetPercentalValue(int whole, int part) => (float) part / whole * 100;

        private string GetNameOfRace(Race race)
        {
            if (race == Race.Human)
            {
                return "Human";
            }
            else if (race == Race.Elf)
            {
                return "Elf";
            }            
            else if (race == Race.Orc)
            {
                return "Orc";
            }

            return "Undefined";
        }

        private string GetNameOfGender(Gender gender)
        {
            if (gender == Gender.Male)
            {
                return "Male";
            }
            else if (gender == Gender.Female)
            {
                return "Female";
            }

            return "Battle helicopter";
        }

        private string GetNameOfBattleClass(BattleClass battleClass)
        {
            if (battleClass == BattleClass.Swordsman)
            {
                return "Swordsman";
            }
            else if (battleClass == BattleClass.Archer)
            {
                return "Archer";
            }            
            else if (battleClass == BattleClass.Mage)
            {
                return "Mage";
            }

            return "Pleb";
        }
    }
}
