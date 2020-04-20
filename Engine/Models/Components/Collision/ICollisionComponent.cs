using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components
{
    [Flags]
    public enum CollisionType
    {
        None = 0,
        Solid = 1 << 0,
        Dynamic = 1 << 1
    }
    public interface ICollisionComponent : IGameComponent
    {
        List<uint> CollidingWith { get; set; }
        bool IsDynamic { get; set; }
        bool IsSolid { get; set; }
    }
}
