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
            _ownerState.TurnDecided = true;
        }
    }
}
