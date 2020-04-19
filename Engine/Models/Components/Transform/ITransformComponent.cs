using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.Components
{
    public interface ITransformComponent : IGameComponent
    {
        int ZIndex { get; set; }
        float ScaleX { get; set; }
        float ScaleY { get; set; }
        Vector2 Position { get; set; }
        Vector2 Rotation { get; set; }
    }
}
