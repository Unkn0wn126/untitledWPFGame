using Engine.Models.Components.Script.BattleState;
using System;
using System.Collections.Generic;
using TimeUtils;

namespace Engine.Models.Components.Script
{
    /// <summary>
    /// A script component used to
    /// control the AI behavior
    /// during the battle
    /// </summary>
    public class AIBattleScript : IScriptComponent
    {
        private readonly BattleStateMachine _ownerState;
        private readonly GameTime _gameTime;
        private readonly Random _rnd;
        private readonly List<int> _possibleMovements;

        private bool _strategySet;
        private float _timeElapsed;

        public AIBattleScript(BattleStateMachine ownerState, GameTime gameTime)
        {
            _rnd = new Random();
            _gameTime = gameTime;
            _ownerState = ownerState;
            _possibleMovements = new List<int>((int[])(Enum.GetValues(typeof(MovementType))));
            _possibleMovements.Remove((int)MovementType.Heal);
        }

        public void Update()
        {
            if (!_strategySet)
            {
                int initialAction = _possibleMovements[_rnd.Next(_possibleMovements.Count)];
                if (_ownerState.IsCloseToDeath() && _rnd.Next(101) <= 25)
                    initialAction = (int)MovementType.Heal;

                while (!IsMovementValid((MovementType)initialAction))
                {
                    initialAction = _possibleMovements[_rnd.Next(_possibleMovements.Count)];
                    if ((MovementType)initialAction == MovementType.None && !_ownerState.IsCloseToExhaustion())
                        initialAction = _possibleMovements[_rnd.Next(_possibleMovements.Count)];
                }

                _ownerState.MovementType = (MovementType)initialAction;
                SetAttackDirection(_ownerState.MovementType);
                SetAttackType(_ownerState.AttackDirection);
                _strategySet = true;
            }
            else
                _timeElapsed += _gameTime.DeltaTimeInSeconds;

            CheckTheThrillTimer();
        }

        /// <summary>
        /// Checks if the time has come
        /// to come out with the action.
        /// Used for dramatic purposes.
        /// </summary>
        private void CheckTheThrillTimer()
        {
            if (_timeElapsed >= 1)
            {
                _strategySet = false;
                _timeElapsed = 0;
                _ownerState.TurnDecided = true;
            }
        }

        /// <summary>
        /// Checks if the chosen movement is valid
        /// under the current circumstances
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns></returns>
        private bool IsMovementValid(MovementType movementType)
        {
            if (movementType == MovementType.Attack)
            {
                return _ownerState.CanAttack();
            }
            else if (movementType == MovementType.Heal)
            {
                return _ownerState.CanHeal();
            }

            return true;
        }

        /// <summary>
        /// Sets the type of the attack
        /// int the corresponding
        /// battle state machine
        /// </summary>
        /// <param name="direction"></param>
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

        /// <summary>
        /// Sets the direction of the attack
        /// in the corresponding battle
        /// state machine
        /// </summary>
        /// <param name="movementType"></param>
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
