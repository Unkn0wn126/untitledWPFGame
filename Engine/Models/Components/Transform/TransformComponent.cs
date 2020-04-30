using System.Numerics;

namespace Engine.Models.Components
{
    public class TransformComponent : ITransformComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public int ZIndex { get; set; }

        public TransformComponent(Vector2 position, float scaleX, float scaleY, Vector2 rotation, int zIndex)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            Position = position;
            Rotation = rotation;
            ZIndex = zIndex;
        }
    }
}
