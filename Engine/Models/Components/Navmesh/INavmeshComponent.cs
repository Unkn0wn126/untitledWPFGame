using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Navmesh
{
    public interface INavmeshComponent : IGameComponent
    {
        public bool LeadsDown { get; set; }
        public bool LeadsUp { get; set; }
        public bool LeadsLeft { get; set; }
        public bool LeadsRight { get; set; }
    }
}
