using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components
{
    /// <summary>
    /// Flags representing
    /// the types of collision
    /// </summary>
    [Flags]
    public enum CollisionType
    {
        None = 0,
        Solid = 1 << 0,
        Dynamic = 1 << 1
    }

    /// <summary>
    /// Component used to keep
    /// track of entity collisions
    /// </summary>
    public interface ICollisionComponent : IGameComponent
    {
        List<uint> CollidingWith { get; set; }
        bool IsDynamic { get; set; }
        bool IsSolid { get; set; }
    }
}
