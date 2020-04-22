using Engine.Models.Components;
using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
using Engine.Processors;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.ViewModels
{
    public interface IGame
    {
        GameStateMachine State { get; set; }
        IScene CurrentScene { get; set; }
        ISceneManager SceneManager { get; set; }
        void Update();
        void UpdateGraphics();
        void UpdateProcessorContext();
    }
}
