using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components
{
    public interface ICollisionComponent : IGameComponent
    {
        public List<uint> CollidingWith { get; set; }
        public bool IsDynamic { get; set; }
        public bool IsSolid { get; set; }
    }
}
