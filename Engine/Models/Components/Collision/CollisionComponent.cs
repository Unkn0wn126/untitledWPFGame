using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Collision
{
    public class CollisionComponent : ICollisionComponent
    {
        public bool IsSolid { get; set; }
        private int _id;
        public List<ICollisionComponent> CollidingWith { get; set; }
        public bool IsDynamic { get; set; }

        public CollisionComponent(int id, bool dynamic)
        {
            _id = id;
            IsDynamic = dynamic;
            CollidingWith = new List<ICollisionComponent>();
        }
    }
}
