using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Life
{
    [Serializable]
    public class LifeComponent : ILifeComponent
    {
        private int _stamina;
        private int _hp;
        private int _mp;
        public bool IsPlayer { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }
        public BattleClass BattleClass { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int MaxStamina { get; set; }
        public int Stamina { get { return _stamina; } 
            set {
                if (value < 0)
                {
                    _stamina = 0;
                }
                if (value > MaxStamina)
                {
                    _stamina = MaxStamina;
                }
            } 
        }
        public int MaxHP { get; set; }
        public int HP { get { return _hp; } 
            set {
                if (value < 0)
                {
                    _hp = 0;
                }
                else if (value > MaxHP)
                {
                    _hp = MaxHP;
                }
                else
                {
                    _hp = value;
                }
            } 
        }
        public int MaxMP { get; set; }
        public int MP { get { return _mp; } 
            set {
                if (value < 0)
                {
                    _mp = 0;
                }
                else if (value > MaxMP)
                {
                    _mp = MaxMP;
                }
                else
                {
                    _mp = value;
                }
            } 
        }

        public int AttributePoints { get; set; }
        public int CurrentXP { get; set; }
        public int NextLevelXP { get; set; }
        public int CurrentLevel { get; set; }

        public LifeComponent()
        {
            IsPlayer = false;
            Name = "NoName";
            Gender = Gender.Male;
            Race = Race.Human;
            BattleClass = BattleClass.Swordsman;
            Strength = 10;
            Stamina = 10;
            Agility = 10;
            Intelligence = 10;
            MaxStamina = 100;
            Stamina = 100;
            MaxHP = 100;
            HP = 100;
            MaxMP = 100;
            MP = 100;
            AttributePoints = 0;
            CurrentXP = 0;
            CurrentLevel = 1;
            NextLevelXP = CurrentLevel * 10;
        }
    }
}
