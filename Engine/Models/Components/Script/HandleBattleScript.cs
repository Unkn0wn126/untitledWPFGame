using Engine.Models.Components.Life;
using Engine.Models.Components.Script.BattleState;
using Engine.Models.Scenes;
using System;

namespace Engine.Models.Components.Script
{
    /// <summary>
    /// Script used to manipulate
    /// the flow of the battle.
    /// Determines who starts and
    /// whose turn it is after each turn.
    /// </summary>
    public class HandleBattleScript : IScriptComponent
    {
        private readonly ILifeComponent _playerLife;
        private readonly ILifeComponent _enemyLife;
        private readonly BattleStateMachine _playerBattleState;
        private readonly BattleStateMachine _enemyBattleState;
        private readonly Random _rnd;

        private readonly GameEnd _onPlayerDead;
        private readonly GameEnd _onPlayerWon;

        private readonly MessageProcessor _messageProcessor;

        public HandleBattleScript(ILifeComponent playerLife, ILifeComponent enemyLife, 
            BattleStateMachine playerBattleState, BattleStateMachine enemyBattleState, 
            GameEnd onPlayerDead, GameEnd onPlayerWon, MessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor;
            _onPlayerDead = onPlayerDead;
            _onPlayerWon = onPlayerWon;
            _playerLife = playerLife;
            _enemyLife = enemyLife;
            _playerBattleState = playerBattleState;
            _enemyBattleState = enemyBattleState;
            _rnd = new Random();
            SetStartingStates();
        }

        /// <summary>
        /// Randomly chooses who should
        /// be the first one to attack
        /// and sets the statest accordingly
        /// </summary>
        private void SetStartingStates()
        {
            int whoStarts = _rnd.Next(41);
            if (whoStarts <= 20)
            {
                _playerBattleState.IsOnTurn = true;
                _playerBattleState.TurnDecided = false;
                _enemyBattleState.IsOnTurn = false;
                _enemyBattleState.TurnDecided = false;
                _messageProcessor.Invoke($"{_playerLife.Name} starts the fight");
            }
            else
            {
                _enemyBattleState.IsOnTurn = true;
                _enemyBattleState.TurnDecided = false;
                _playerBattleState.IsOnTurn = false;
                _playerBattleState.TurnDecided = false;
                _messageProcessor.Invoke($"{_enemyLife.Name} starts the fight");
            }
        }

        public void Update()
        {
            if (CheckBattleEndConditions())
            {
                return;
            }
            if (_playerBattleState.IsOnTurn && _playerBattleState.TurnDecided)
            {
                _messageProcessor.Invoke("\n" + _playerBattleState.ProcessState(_enemyLife));
                _playerBattleState.TurnDecided = false;
                _enemyBattleState.IsOnTurn = true;
                _enemyBattleState.TurnDecided = false;
            }
            else if (_enemyBattleState.IsOnTurn && _enemyBattleState.TurnDecided)
            {
                _messageProcessor.Invoke("\n" + _enemyBattleState.ProcessState(_playerLife));
                _enemyBattleState.TurnDecided = false;
                _playerBattleState.IsOnTurn = true;
                _playerBattleState.TurnDecided = false;
            }
            else
            {
                _messageProcessor.Invoke(string.Empty);
            }
        }

        /// <summary>
        /// Checks if the battle
        /// should continue.
        /// If the player is dead,
        /// game over is invoked.
        /// If the enemy is dead,
        /// battle victory is
        /// invoked.
        /// </summary>
        /// <returns></returns>
        private bool CheckBattleEndConditions()
        {
            if (_playerLife.HP <= 0)
            {
                _onPlayerDead.Invoke();
                return true;
            }
            else if (_enemyLife.HP <= 0)
            {
                // Level the player up after victory
                _playerLife.CurrentXP += _enemyLife.CurrentLevel * 50;
                _onPlayerWon.Invoke();
                return true;
            }

            return false;
        }
    }
}
