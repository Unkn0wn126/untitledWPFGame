using Engine.Models.Components;
using Engine.Models.GameStateMachine;
using Engine.Models.MovementStateStrategies;
using Engine.Models.Scenes;
using Engine.ResourceConstants.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.ViewModels
{
    public interface IGame
    {
        public GameStateMachine State { get; set; }
        public ImagePaths ImgPaths { get; set; }
        public List<IGraphicsComponent> GraphicsComponents { get; set; }
        public IScene CurrentScene { get; set; }
        public void Update();
        public void HandleUserInput(IMovementStrategy newState);
    }
}
