using Engine.Models.Components.Life;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Script.BattleState
{
    public enum AttackDirection
    {
        None,
        Head,
        Left,
        Right,
        Bottom
    }
    public enum AttackType
    {
        None,
        Normal,
        Heavy
    }
    public enum MovementType
    {
        None,
        Attack,
        Heal
    }
    public class BattleStateMachine
    {
        public bool IsOnTurn { get; set; }
        public bool TurnDecided { get; set; }
        public AttackType AttackType { get; set; }
        public AttackDirection AttackDirection { get; set; }
        public MovementType MovementType { get; set; }

        private ILifeComponent _owner { get; set; }

        private Random _rnd;

        public BattleStateMachine(ILifeComponent owner)
        {
            _owner = owner;
            _rnd = new Random();
        }

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
                returnMessage = $"{_owner.Name} is passing this round\n" + ReplenishStaminaAndMP();
            }
            else
            {
                returnMessage = $"You should not see this...";
            }

            IsOnTurn = false;
            return returnMessage;
        }

        public bool CanAttack()
        {
            return _owner.Stamina >= DetermineStaminaCost();
        }

        public bool CanHeal()
        {
            return _owner.MP >= DetermineHealCost();
        }

        private float DetermineStaminaCost()
        {
            float baseStaminaDecreaseValue = 10f;
            baseStaminaDecreaseValue -= _owner.Agility / 2f;

            return baseStaminaDecreaseValue;
        }

        private float DetermineHealCost()
        {
            float baseMPCost = 30;
            baseMPCost -= _owner.Intelligence / 2f;

            return baseMPCost;
        }

        private string DecreaseStamina()
        {
            float baseStaminaDecreaseValue = DetermineStaminaCost();

            _owner.Stamina -= (int)baseStaminaDecreaseValue;

            return $"{_owner.Name} used {baseStaminaDecreaseValue} stamina";
        }

        public bool IsCloseToDeath()
        {
            return _owner.HP <= _owner.MaxHP / 4f;
        }

        private string ReplenishHP()
        {
            float baseMPCost = DetermineHealCost();
            float baseHealValue = 15;
            baseHealValue += _owner.Intelligence / 2f;
            _owner.HP += (int)baseHealValue;
            _owner.MP -= (int)baseMPCost;

            return $"{_owner.Name} healed {baseHealValue} HP for {baseMPCost} MP!";
        }

        private string ReplenishStaminaAndMP()
        {
            float baseStaminaAddition = 5f;
            baseStaminaAddition += _owner.Agility / 2f;
            _owner.Stamina += (int)baseStaminaAddition;

            float baseMPAddition = 5f;
            baseMPAddition += _owner.Intelligence / 2f;
            _owner.MP += (int)baseMPAddition;
            return $"{_owner.Name} replenished {baseStaminaAddition} stamina and {baseMPAddition} MP!";
        }

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

        private float DetermineAttackDamage(ILifeComponent target)
        {
            float baseAttackDamage = _owner.Strength + (_owner.Stamina / 10f) + (_owner.Agility / 10f);
            float headDamage = 10;
            float bottomDamage = 5;
            float sideDamage = 3;

            if (AttackType == AttackType.Heavy)
            {
                baseAttackDamage += _owner.Strength;
            }

            if (AttackDirection == AttackDirection.Head)
            {
                baseAttackDamage += headDamage;
            }
            else if (AttackDirection == AttackDirection.Bottom)
            {
                baseAttackDamage += bottomDamage;
            }
            else if (AttackDirection == AttackDirection.Left || AttackDirection == AttackDirection.Right)
            {
                baseAttackDamage += sideDamage;
            }

            baseAttackDamage -= target.Strength / 10f;
            float levelDifference = _owner.CurrentLevel - target.CurrentLevel;
            baseAttackDamage += levelDifference * 10;

            return baseAttackDamage;
        }

        private float DetermineAttackMissChance(ILifeComponent target)
        {
            float baseMissChance = 100;
            float headMissChance = 10;
            float bottomMissChance = 5;
            float sideMissChance = 3;

            if (AttackType == AttackType.Heavy)
            {
                baseMissChance += baseMissChance - (_owner.Strength + (_owner.Stamina / 10f));
            }

            baseMissChance += ((target.Agility / 2f) + (target.Stamina / 10f));

            if (AttackDirection == AttackDirection.Head)
            {
                baseMissChance += headMissChance;
            }
            else if (AttackDirection == AttackDirection.Bottom)
            {
                baseMissChance += bottomMissChance;
            }
            else if (AttackDirection == AttackDirection.Left || AttackDirection == AttackDirection.Right)
            {
                baseMissChance += sideMissChance;
            }

            return baseMissChance;
        }
    }
}
