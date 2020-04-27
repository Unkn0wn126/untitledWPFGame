using Engine.Models.Components.Script.BattleState;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Script
{
    public class AIBattleScript : IScriptComponent
    {
        private BattleStateMachine _ownerState;
        private Random _rnd;
        public AIBattleScript(BattleStateMachine ownerState)
        {
            _rnd = new Random();
            _ownerState = ownerState;
        }
        public void Update()
        {
            int initialAction = _rnd.Next(Enum.GetValues(typeof(MovementType)).Length);
            _ownerState.MovementType = (MovementType)initialAction;
            SetAttackDirection(_ownerState.MovementType);
            SetAttackType(_ownerState.AttackDirection);
            _ownerState.TurnDecided = true;
        }

        private void SetAttackType(AttackDirection direction)
        {
            if (direction == AttackDirection.None)
            {
                _ownerState.AttackType = AttackType.None;
            }
            else
            {
                int attackType = _rnd.Next(1, Enum.GetValues(typeof(AttackType)).Length);
                _ownerState.AttackType = (AttackType)attackType;
            }
        }

        private void SetAttackDirection(MovementType movementType)
        {
            if (movementType == MovementType.Heal)
            {
                _ownerState.AttackDirection = AttackDirection.None;
            }
            else
            {
                int attackDirection = _rnd.Next(1, Enum.GetValues(typeof(AttackDirection)).Length);
                _ownerState.AttackDirection = (AttackDirection)attackDirection;
            }
        }
    }
}
