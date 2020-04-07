using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.RigidBody
{
    public interface IRigidBodyComponent : IGameComponent
    {
        float ForceX { get; set; }
        float ForceY { get; set; }
    }
}
