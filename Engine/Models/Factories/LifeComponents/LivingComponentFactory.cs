using Engine.Models.Components.Life;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Factories.LifeComponents
{
    public static class LivingComponentFactory
    {
        private static Random _random = new Random();
        private static List<string> _maleNames = new List<string> 
        { 
            "Prak",
            "Larry",
            "Dr. Strange",
            "Doom Guy",
            "The Chief",
            "The Chosen One",
            "Liam",
            "Shay Patric Cormac",
            "Bayek of Siwa",
            "Ezio Auditore Da Firenze",
            "Altaïr Ibn-La'Ahad",
            "Achilles Davenport",
            "Connor",
            "Haytham Kenway",
            "Edward Kenway",
            "Ratonhnhaké:ton",
            "Jacob Frye"
        };
        private static List<string> _femaleNames = new List<string> 
        { 
            "Triss",
            "Yennefer",
            "Karen",
            "Emma",
            "Olivia",
            "Ava",
            "Isabella",
            "Sophia",
            "Charlotte",
            "Mia",
            "Amelia",
            "Nikki",
            "Claudia Auditore",
            "Christina Vespucci",
            "Sofia Sartor",
            "Evie Frye",
            "Aya"
        };
        public static ILifeComponent GenerateLifeComponent(string name, Race race, Gender gender, BattleClass battleClass)
        {
            int strength = DetermineStrength(race, gender, battleClass);
            int agility = DetermineAgility(race, gender, battleClass);
            int intelligence = DetermineIntelligence(race, gender, battleClass);
            int maxHP = DetermineMaxHP(strength);
            int maxStamina = DetermineMaxStamina(strength);
            int maxMP = DetermineMaxMP(intelligence);
            return new LifeComponent()
            {
                Name = name,
                Strength = strength,
                Agility = agility,
                Intelligence = intelligence,
                BattleClass = battleClass,
                Gender = gender,
                Race = race,
                MaxHP = maxHP,
                HP = maxHP,
                MaxMP = maxMP,
                MP = maxMP,
                MaxStamina = maxStamina,
                Stamina = maxStamina
            };
        }

        private static int DetermineStrength(Race race, Gender gender, BattleClass battleClass)
        {
            int baseValue = 10;
            if (race == Race.Orc)
            {
                baseValue += 10;
            }
            else if (race == Race.Human)
            {
                baseValue += 5;
            }

            if (gender == Gender.Male)
            {
                baseValue += 5;
            }

            if (battleClass == BattleClass.Swordsman)
            {
                baseValue += 10;
            }

            return baseValue;
        }

        private static int DetermineAgility(Race race, Gender gender, BattleClass battleClass)
        {
            int baseValue = 10;
            if (race == Race.Elf)
            {
                baseValue += 10;
            }
            else if (race == Race.Human)
            {
                baseValue += 5;
            }

            if (gender == Gender.Female)
            {
                baseValue += 10;
            }

            if (battleClass == BattleClass.Archer)
            {
                baseValue += 10;
            }

            return baseValue;
        }

        private static int DetermineIntelligence(Race race, Gender gender, BattleClass battleClass)
        {
            int baseValue = 10;
            if (race == Race.Human)
            {
                baseValue += 10;
            }
            else if (race == Race.Elf)
            {
                baseValue += 5;
            }

            if (battleClass == BattleClass.Mage)
            {
                baseValue += 10;
            }

            return baseValue;
        }

        private static int DetermineMaxHP(int strength)
        {
            int baseValue = 100;
            return baseValue + strength / 5;
        }

        private static int DetermineMaxStamina(int agility)
        {
            int baseValue = 100;
            return baseValue + agility / 5;
        }

        private static int DetermineMaxMP(int intelligence)
        {
            int baseValue = 100;
            return baseValue + intelligence / 5;
        }

        public static Gender RandomlyGenerateGender()
        {
            return (Gender)_random.Next(Enum.GetValues(typeof(Gender)).Length);
        }

        public static Race RandomlyGenerateRace()
        {
            return (Race)_random.Next(Enum.GetValues(typeof(Race)).Length);
        }

        public static BattleClass RandomlyGenerateBattleClass()
        {
            return (BattleClass)_random.Next(Enum.GetValues(typeof(BattleClass)).Length);
        }

        public static string GenerateFittingName(Gender gender)
        {
            if (gender == Gender.Male)
            {
                int index = _random.Next(_maleNames.Count);
                return _maleNames[index];
            }
            else
            {
                int index = _random.Next(_femaleNames.Count);
                return _femaleNames[index];
            }
        }
    }
}
