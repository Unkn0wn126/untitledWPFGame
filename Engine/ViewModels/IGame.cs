using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;

namespace Engine.ViewModels
{
    public interface IGame
    {
        GameStateMachine State { get; set; }
        ISceneManager SceneManager { get; set; }
        void Update();
        void UpdateGraphics();
        void UpdateProcessorContext();
    }
}
