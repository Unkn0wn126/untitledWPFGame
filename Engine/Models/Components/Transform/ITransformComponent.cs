using System.Numerics;

namespace Engine.Models.Components
{
    /// <summary>
    /// Game component used to
    /// keep track of space properties
    /// of a given entity.
    /// Use this to move entity around.
    /// </summary>
    public interface ITransformComponent : IGameComponent
    {
        int ZIndex { get; set; }
        float ScaleX { get; set; }
        float ScaleY { get; set; }
        Vector2 Position { get; set; }
        Vector2 Rotation { get; set; }
    }
}
