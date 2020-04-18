using Engine.Models.Components;
using Engine.Models.GameStateMachine;
using Engine.Models.MovementStateStrategies;
using Engine.Models.Scenes;
using Engine.Processors;
using Engine.ResourceConstants.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.ViewModels
{
    public interface IGame
    {
        GameStateMachine State { get; set; }
        ImagePaths ImgPaths { get; set; }
        IScene CurrentScene { get; set; }
        void Update();
        void HandleUserInput();
        void UpdateGraphics();
    }
}
