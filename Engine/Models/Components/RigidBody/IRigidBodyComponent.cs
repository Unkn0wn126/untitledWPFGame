using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.RigidBody
{
    public interface IRigidBodyComponent : IGameComponent
    {
        public float ForceX { get; set; }
        public float ForceY { get; set; }
    }
}
