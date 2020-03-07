using Engine.Models.Components;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.ViewModels
{
    public interface IGame
    {
        public List<IGraphicsComponent> GraphicsComponents { get; set; }
        public IScene CurrentScene { get; set; }
        public void Update();
        public void HandleUserInput(MovementState newState);
    }
}
