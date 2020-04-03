using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.Components
{
    public interface ITransformComponent : IGameComponent
    {
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set; }
    }
}
