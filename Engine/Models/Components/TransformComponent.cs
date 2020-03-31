using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.Components
{
    public class TransformComponent : ITransformComponent
    {
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set; }

        public TransformComponent(Vector2 position, float scaleX, float scaleY, Vector2 rotation)
        {
            Position = position;
            ScaleX = scaleX;
            ScaleY = scaleY;
            Rotation = rotation;
        }
    }
}
