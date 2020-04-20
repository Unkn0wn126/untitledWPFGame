using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Collision
{
    public class CollisionComponent : ICollisionComponent
    {
        public bool IsSolid { get; set; }
        public List<uint> CollidingWith { get; set; }
        public bool IsDynamic { get; set; }

        public CollisionComponent(bool dynamic)
        {
            IsDynamic = dynamic;
            CollidingWith = new List<uint>();
        }
    }
}
