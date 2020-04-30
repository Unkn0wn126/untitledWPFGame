using Engine.Models.Components;
using Engine.Models.Components.Life;
using Engine.Models.Components.Script.BattleState;

namespace Engine.Models.Scenes
{
    public delegate void MessageProcessor(string message);

    /// <summary>
    /// Used as a mediator between
    /// the game logic and the GUI
    /// when dealing with the
    /// battle screen
    /// </summary>
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
