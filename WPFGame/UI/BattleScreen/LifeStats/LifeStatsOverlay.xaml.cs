using Engine.Models.Components.Life;
using System.Windows.Controls;

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

        /// <summary>
        /// Updates the state of
        /// the stats to display
        /// </summary>
        /// <param name="entityStats"></param>
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

        /// <summary>
        /// Gets the percental value
        /// in relation to the max value
        /// </summary>
        /// <param name="whole"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private float GetPercentalValue(int whole, int part) => (float) part / whole * 100;

        /// <summary>
        /// Helper method to convert
        /// enum values to a more
        /// user friendly representation
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method to convert
        /// enum values to a more
        /// user friendly representation
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method to convert
        /// enum values to a more
        /// user friendly representation
        /// </summary>
        /// <param name="battleClass"></param>
        /// <returns></returns>
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
