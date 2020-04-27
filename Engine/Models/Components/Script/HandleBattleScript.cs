using Engine.Models.Components.Life;
using Engine.Models.Components.Script.BattleState;
using Engine.Models.Scenes;
using System;
using System.Diagnostics;

namespace Engine.Models.Components.Script
{
    public class HandleBattleScript : IScriptComponent
    {
        private ILifeComponent _playerLife;
        private ILifeComponent _enemyLife;
        private BattleStateMachine _playerBattleState;
        private BattleStateMachine _enemyBattleState;
        private Random _rnd;

        GameEnd _onPlayerDead;
        GameEnd _onPlayerWon;

        public HandleBattleScript(ILifeComponent playerLife, ILifeComponent enemyLife, BattleStateMachine playerBattleState, BattleStateMachine enemyBattleState, GameEnd onPlayerDead, GameEnd onPlayerWon)
        {
            _onPlayerDead = onPlayerDead;
            _onPlayerWon = onPlayerWon;
            _playerLife = playerLife;
            _enemyLife = enemyLife;
            _playerBattleState = playerBattleState;
            _enemyBattleState = enemyBattleState;
            _rnd = new Random();
            SetStartingStates();
        }

        private void SetStartingStates()
        {
            int whoStarts = _rnd.Next(41);
            if (whoStarts <= 20)
            {
                _playerBattleState.IsOnTurn = true;
                _playerBattleState.TurnDecided = false;
                _enemyBattleState.IsOnTurn = false;
                _enemyBattleState.TurnDecided = false;
            }
            else
            {
                _enemyBattleState.IsOnTurn = true;
                _enemyBattleState.TurnDecided = false;
                _playerBattleState.IsOnTurn = false;
                _playerBattleState.TurnDecided = false;
            }
        }

        public void Update()
        {
            Trace.WriteLine($"Player turn: {_playerBattleState.IsOnTurn}; Enemy turn: {_enemyBattleState.IsOnTurn}");
            if (CheckBattleEndConditions())
            {
                return;
            }
            if (_playerBattleState.IsOnTurn && _playerBattleState.TurnDecided)
            {
                _playerBattleState.ProcessState(_enemyLife);
                _playerBattleState.TurnDecided = false;
                _enemyBattleState.IsOnTurn = true;
                _enemyBattleState.TurnDecided = false;
            }
            else if (_enemyBattleState.IsOnTurn && _enemyBattleState.TurnDecided)
            {
                _enemyBattleState.ProcessState(_playerLife);
                _enemyBattleState.TurnDecided = false;
                _playerBattleState.IsOnTurn = true;
                _playerBattleState.TurnDecided = false;
            }
        }

        private bool CheckBattleEndConditions()
        {
            if (_playerLife.HP <= 0)
            {
                _onPlayerDead.Invoke();
                return true;
            }
            else if (_enemyLife.HP <= 0)
            {
                _onPlayerWon.Invoke();
                return true;
            }

            return false;
        }
    }
}
