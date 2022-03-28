namespace Engine.Models.Components.Life
{
    /// <summary>
    /// Represents the gender of a character
    /// </summary>
    public enum Gender
    {
        Male,
        Female
    }

    /// <summary>
    /// Represents the race of a character
    /// </summary>
    public enum Race
    {
        Human,
        Elf,
        Orc
    }

    /// <summary>
    /// Represents the battle class of a character
    /// </summary>
    public enum BattleClass
    {
        Swordsman,
        Archer,
        Mage
    }

    /// <summary>
    /// Component used to keep track
    /// of all the attributes of a living entity.
    /// Basically the core of the RPG features
    /// </summary>
    public interface ILifeComponent : IGameComponent
    {
        bool IsPlayer { get; set; }
        string Name { get; set; }
        Gender Gender { get; set; }
        Race Race { get; set; }
        BattleClass BattleClass { get; set; }
        int Strength { get; set; }
        int Agility { get; set; }
        int Intelligence { get; set; }
        int MaxStamina { get; set; }
        int Stamina { get; set; }
        int MaxHP { get; set; }
        int HP { get; set; }        
        int MaxMP { get; set; }
        int MP { get; set; }
        int AttributePoints { get; set; }
        int CurrentXP { get; set; }
        int NextLevelXP { get; set; }
        int CurrentLevel { get; set; }
    }
}
