using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.RigidBody
{
    public class RigidBodyComponent : IRigidBodyComponent
    {
        public float ForceX { get; set; }
        public float ForceY { get; set; }

        public RigidBodyComponent()
        {
            ForceX = 0f;
            ForceY = 0f;
        }
    }
}
