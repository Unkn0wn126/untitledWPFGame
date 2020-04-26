using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Navmesh
{
    [Flags]
    public enum NavmeshContinues
    {
        None = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
    }
    public interface INavmeshComponent : IGameComponent
    {
        public bool LeadsDown { get; set; }
        public bool LeadsUp { get; set; }
        public bool LeadsLeft { get; set; }
        public bool LeadsRight { get; set; }
    }
}
