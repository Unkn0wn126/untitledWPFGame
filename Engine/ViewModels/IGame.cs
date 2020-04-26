using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
using System.Collections.Generic;

namespace Engine.ViewModels
{
    public interface IGame
    {
        GameStateMachine State { get; set; }
        ISceneManager SceneManager { get; set; }
        bool UpdateInProgress { get; set; }
        void InitializeGame(List<byte[]> metaScenes, int currentIndex);
        void Update();
        void UpdateGraphics();
        void UpdateProcessorContext();
    }
}
