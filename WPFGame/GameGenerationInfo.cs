using Engine.Models.Components.Life;

namespace WPFGame
{
    /// <summary>
    /// Helper class to hold
    /// the info needed to
    /// generate a new game
    /// </summary>
    public class GameGenerationInfo
    {
        public int MinOnX { get; set; }
        public int MaxOnX { get; set; }
        public int MinOnY { get; set; }
        public int MaxOnY { get; set; }
        public int NumberOfLevels { get; set; }
        public Race PlayerRace { get; set; }
        public Gender PlayerGender { get; set; }
        public BattleClass BattleClass { get; set; }
        public string PlayerName { get; set; }
    }
}
