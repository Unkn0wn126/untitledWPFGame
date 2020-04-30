using Engine.Models.Components.Life;
using System;
using System.Collections.Generic;

namespace Engine.Models.Factories.LifeComponents
{
    /// <summary>
    /// Factory class for RPG
    /// character generation
    /// </summary>
    public static class LivingComponentFactory
    {
        private static readonly Random _random = new Random();

        private static readonly List<string> _maleNames = new List<string> 
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

        private static readonly List<string> _femaleNames = new List<string> 
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

        /// <summary>
        /// Generates a life component
        /// based on the provided properties
        /// </summary>
        /// <param name="name"></param>
        /// <param name="race"></param>
        /// <param name="gender"></param>
        /// <param name="battleClass"></param>
        /// <returns></returns>
        public static ILifeComponent GenerateLifeComponent(string name, Race race, Gender gender, BattleClass battleClass)
        {
            int strength = DetermineStrength(race, gender, battleClass);
            int agility = DetermineAgility(race, gender, battleClass);
            int intelligence = DetermineIntelligence(race, battleClass);
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

        /// <summary>
        /// Determines the strength
        /// of a character based on
        /// its race, gender and battle class
        /// </summary>
        /// <param name="race"></param>
        /// <param name="gender"></param>
        /// <param name="battleClass"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines the character agility
        /// based on its race, gender and battleclass
        /// </summary>
        /// <param name="race"></param>
        /// <param name="gender"></param>
        /// <param name="battleClass"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines the character intelligence
        /// based on its race and battle class.
        /// #genderEquality :)
        /// </summary>
        /// <param name="race"></param>
        /// <param name="gender"></param>
        /// <param name="battleClass"></param>
        /// <returns></returns>
        private static int DetermineIntelligence(Race race, BattleClass battleClass)
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

        /// <summary>
        /// Determines the max HP
        /// of a charater based on
        /// its strength
        /// </summary>
        /// <param name="strength"></param>
        /// <returns></returns>
        private static int DetermineMaxHP(int strength)
        {
            int baseValue = 100;
            return baseValue + strength / 5;
        }

        /// <summary>
        /// Determines the max stamina
        /// of a charater based on
        /// its agility
        /// </summary>
        /// <param name="agility"></param>
        /// <returns></returns>
        private static int DetermineMaxStamina(int agility)
        {
            int baseValue = 100;
            return baseValue + agility / 5;
        }

        /// <summary>
        /// Determines the max MP
        /// of a charater based on
        /// its intelligence
        /// </summary>
        /// <param name="intelligence"></param>
        /// <returns></returns>
        private static int DetermineMaxMP(int intelligence)
        {
            int baseValue = 100;
            return baseValue + intelligence / 5;
        }

        /// <summary>
        /// Randomly generates a gender.
        /// There are only 2 implemented.
        /// </summary>
        /// <returns></returns>
        public static Gender RandomlyGenerateGender()
        {
            return (Gender)_random.Next(Enum.GetValues(typeof(Gender)).Length);
        }

        /// <summary>
        /// Randomly generates a race.
        /// </summary>
        /// <returns></returns>
        public static Race RandomlyGenerateRace()
        {
            return (Race)_random.Next(Enum.GetValues(typeof(Race)).Length);
        }

        /// <summary>
        /// Randomly generates a battle class
        /// </summary>
        /// <returns></returns>
        public static BattleClass RandomlyGenerateBattleClass()
        {
            return (BattleClass)_random.Next(Enum.GetValues(typeof(BattleClass)).Length);
        }

        /// <summary>
        /// Generates a name based on
        /// the character's gender
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
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
