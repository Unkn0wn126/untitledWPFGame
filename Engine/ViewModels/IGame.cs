using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
using System.Collections.Generic;

namespace Engine.ViewModels
{
    public interface IGame
    {
        GameStateMachine State { get; set; }
        ISceneManager SceneManager { get; set; }
        void InitializeGame(List<byte[]> metaScenes);
        void Update();
        void UpdateGraphics();
        void UpdateProcessorContext();
    }
}
