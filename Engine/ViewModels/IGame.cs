using Engine.Models.Components;
using Engine.Models.Scenes;
using System.Collections.Generic;

namespace Engine.ViewModels
{
    public interface IGame
    {
        List<IGraphicsComponent> GraphicsComponents { get; set; }
        IScene CurrentScene { get; set; }
        void Update();
        void HandleUserInput(MovementState newState);
    }
}
