using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.Components
{
    public interface IGraphicsComponent : IGameComponent
    {
        public Vector2 Position { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string CurrentImageName { get; set; }
        public List<string> ImageNames { get; set; }
    }
}
