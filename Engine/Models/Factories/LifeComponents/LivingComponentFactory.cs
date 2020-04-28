using Engine.Models.Components.Life;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Factories.LifeComponents
{
    public static class LivingComponentFactory
    {
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
    }
}
