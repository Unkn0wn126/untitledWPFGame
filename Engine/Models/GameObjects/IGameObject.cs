using Engine.Coordinates;
using Engine.Models.Components;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.GameObjects
{
    public interface IGameObject
    {
        public Grid Grid { get; set; }
        public Guid Id { get; set; }
        public ITransformComponent Transform { get; set; }
        public IGraphicsComponent GraphicsComponent {get;set;}
        public void Move(Vector2 newPos);
        public void Update(IScene logicContext);
    }
}
