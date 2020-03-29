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
        public Guid Id { get; set; }
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public IGraphicsComponent GraphicsComponent {get;set;}
        public void Update(IScene logicContext);
    }
}
