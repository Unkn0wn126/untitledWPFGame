using Engine.Models.Components;
using Engine.Models.Components.Life;
using Engine.Models.Components.Script.BattleState;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Scenes
{
    public delegate void MessageProcessor(string message);
    public class BattleSceneMediator
    {
        public ILifeComponent PlayerLife { get; set; }
        public ILifeComponent EnemyLife { get; set; }
        public BattleStateMachine PlayerBattleState { get; set; }
        public IGraphicsComponent PlayerAvatar { get; set; }
        public IGraphicsComponent EnemyAvatar { get; set; }
        public MessageProcessor MessageProcessor;
    }
}
