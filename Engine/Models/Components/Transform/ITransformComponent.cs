using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.Components
{
    public interface ITransformComponent : IGameComponent
    {
        float ScaleX { get; set; }
        float ScaleY { get; set; }
        Vector2 Position { get; set; }
        Vector2 Rotation { get; set; }
    }
}
