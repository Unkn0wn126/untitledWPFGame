using Engine.Models.Components.Life;
using System;

namespace Engine.Models.Components.Script.BattleState
{
    /// <summary>
    /// Used to determine attack direction
    /// </summary>
    public enum AttackDirection
    {
        None,
        Head,
        Left,
        Right,
        Bottom
    }

    /// <summary>
    /// Used to determine attack type
    /// </summary>
    public enum AttackType
    {
        None,
        Normal,
        Heavy
    }

    /// <summary>
    /// Used to determine movement type
    /// </summary>
    public enum MovementType
    {
        None,
        Attack,
        Heal
    }

    /// <summary>
    /// Used to process the desired
    /// battle behavior
    /// </summary>
    public class BattleStateMachine
    {
        private readonly Random _rnd;
        private ILifeComponent Owner { get; set; }

        public bool IsOnTurn { get; set; }
        public bool TurnDecided { get; set; }
        public AttackType AttackType { get; set; }
        public AttackDirection AttackDirection { get; set; }
        public MovementType MovementType { get; set; }

        public BattleStateMachine(ILifeComponent owner)
        {
            Owner = owner;
            _rnd = new Random();
        }

        /// <summary>
        /// Produces an output based
        /// on its inner state
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string ProcessState(ILifeComponent target)
        {
            string returnMessage;
            if (MovementType == MovementType.Attack)
            {
                returnMessage = ProcessAttack(target) + "\n" + DecreaseStamina();
            }
            else if (MovementType == MovementType.Heal)
            {
                returnMessage = ReplenishHP();
            }
            else if (MovementType == MovementType.None)
            {
                returnMessage = $"{Owner.Name} is passing this round\n" + ReplenishStaminaAndMP();
            }
            else
            {
                returnMessage = $"You should not see this...";
            }

            IsOnTurn = false;
            return returnMessage;
        }

        /// <summary>
        /// Checks if stamina is
        /// sufficient for attack
        /// </summary>
        /// <returns></returns>
        public bool CanAttack()
        {
            return Owner.Stamina >= DetermineStaminaCost();
        }

        /// <summary>
        /// Checks if mana is
        /// sufficient for heal
        /// </summary>
        /// <returns></returns>
        public bool CanHeal()
        {
            return Owner.MP >= DetermineHealCost();
        }

        /// <summary>
        /// Calculates the stamina
        /// cost based on the agility
        /// </summary>
        /// <returns></returns>
        private float DetermineStaminaCost()
        {
            float baseStaminaDecreaseValue = 10f;
            baseStaminaDecreaseValue -= Owner.Agility / 2f;

            return baseStaminaDecreaseValue;
        }

        /// <summary>
        /// Calculates the mana
        /// cost based on the intelligence
        /// </summary>
        /// <returns></returns>
        private float DetermineHealCost()
        {
            float baseMPCost = 30;
            baseMPCost -= Owner.Intelligence / 2f;

            return baseMPCost;
        }

        /// <summary>
        /// Decreases the amount of
        /// available stamina for
        /// the owner character
        /// </summary>
        /// <returns></returns>
        private string DecreaseStamina()
        {
            float baseStaminaDecreaseValue = DetermineStaminaCost();

            Owner.Stamina -= (int)baseStaminaDecreaseValue;

            return $"{Owner.Name} used {baseStaminaDecreaseValue} stamina";
        }

        /// <summary>
        /// Checks if the hp
        /// value is lower than 1/4 of
        /// its maximum potential value
        /// </summary>
        /// <returns></returns>
        public bool IsCloseToDeath()
        {
            return Owner.HP <= Owner.MaxHP / 4f;
        }

        /// <summary>
        /// Checks if the stamina
        /// value is lower than 1/4 of
        /// its maximum potential value
        /// </summary>
        /// <returns></returns>
        public bool IsCloseToExhaustion()
        {
            return Owner.Stamina <= Owner.MaxStamina / 4f;
        }

        /// <summary>
        /// Adds back some percentage
        /// of hp for some mp
        /// </summary>
        /// <returns></returns>
        private string ReplenishHP()
        {
            float baseMPCost = DetermineHealCost();
            float baseHealValue = 15;
            baseHealValue += Owner.Intelligence / 2f;
            Owner.HP += (int)baseHealValue;
            Owner.MP -= (int)baseMPCost;

            return $"{Owner.Name} healed {baseHealValue} HP for {baseMPCost} MP!";
        }

        /// <summary>
        /// Replenishes some value
        /// of stamina and mp
        /// </summary>
        /// <returns></returns>
        private string ReplenishStaminaAndMP()
        {
            float baseStaminaAddition = 5f;
            baseStaminaAddition += Owner.Agility / 2f;
            Owner.Stamina += (int)baseStaminaAddition;

            float baseMPAddition = 5f;
            baseMPAddition += Owner.Intelligence / 2f;
            Owner.MP += (int)baseMPAddition;
            return $"{Owner.Name} replenished {baseStaminaAddition} stamina and {baseMPAddition} MP!";
        }

        /// <summary>
        /// Determines the chance of hit
        /// and executes the attack if
        /// the action was determined
        /// successful
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private string ProcessAttack(ILifeComponent target)
        {
            float missChance = DetermineAttackMissChance(target);
            int currentNumber = _rnd.Next(301);
            if (currentNumber > missChance)
            {
                float attackDamage = DetermineAttackDamage(target);
                target.HP -= (int)attackDamage;
                return $"{target.Name} is injured for {attackDamage} HP";
            }
            else
            {
                return $"{target.Name} dodged the attack!";
            }
        }

        /// <summary>
        /// Determines the amount
        /// of damage based on the stats
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private float DetermineAttackDamage(ILifeComponent target)
        {
            float baseAttackDamage = Owner.Strength + (Owner.Stamina / 10f) + (Owner.Agility / 10f);
            float headDamage = 10;
            float bottomDamage = 5;
            float sideDamage = 3;

            if (AttackType == AttackType.Heavy)
                baseAttackDamage += Owner.Strength;

            if (AttackDirection == AttackDirection.Head)
                baseAttackDamage += headDamage;
            else if (AttackDirection == AttackDirection.Bottom)
                baseAttackDamage += bottomDamage;
            else if (AttackDirection == AttackDirection.Left || AttackDirection == AttackDirection.Right)
                baseAttackDamage += sideDamage;

            baseAttackDamage -= target.Strength / 10f;
            float levelDifference = Owner.CurrentLevel - target.CurrentLevel;
            baseAttackDamage += levelDifference * 10;

            return baseAttackDamage;
        }

        /// <summary>
        /// Determines the chance of
        /// an attack missing its target
        /// based on the attributes
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private float DetermineAttackMissChance(ILifeComponent target)
        {
            float baseMissChance = 100;
            float headMissChance = 10;
            float bottomMissChance = 5;
            float sideMissChance = 3;

            if (AttackType == AttackType.Heavy)
                baseMissChance += baseMissChance - (Owner.Strength + (Owner.Stamina / 10f));

            baseMissChance += ((target.Agility / 2f) + (target.Stamina / 10f));

            if (AttackDirection == AttackDirection.Head)
                baseMissChance += headMissChance;
            else if (AttackDirection == AttackDirection.Bottom)
                baseMissChance += bottomMissChance;
            else if (AttackDirection == AttackDirection.Left || AttackDirection == AttackDirection.Right)
                baseMissChance += sideMissChance;

            return baseMissChance;
        }
    }
}
