using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Life
{
    public enum Gender
    {
        Male,
        Female
    }
    public enum Race
    {
        Human,
        Elf,
        Orc
    }
    public enum BattleClass
    {
        Swordsman,
        Archer,
        Mage
    }
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
