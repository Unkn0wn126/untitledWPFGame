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
            if (MovementType == MovementType.Heal)
            {
                int healValue = (int)Math.Ceiling(_owner.Intelligence / 2f);
                _owner.HP += healValue;
                returnMessage = $"{_owner.Name} healed by {healValue} HP";
            }
            else if (MovementType == MovementType.None)
            {
                returnMessage = $"{_owner.Name} is passing this round";
            }
            else
            {
                returnMessage = $"Generic message";
            }

            IsOnTurn = false;
            return returnMessage;
        }
    }
}
