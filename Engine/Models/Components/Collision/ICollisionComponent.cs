using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components
{
    public interface ICollisionComponent : IGameComponent
    {
        List<uint> CollidingWith { get; set; }
        bool IsDynamic { get; set; }
        bool IsSolid { get; set; }
    }
}
