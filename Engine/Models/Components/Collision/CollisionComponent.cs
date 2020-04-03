using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Collision
{
    public class CollisionComponent : ICollisionComponent
    {
        public bool IsSolid { get; set; }
    }
}
