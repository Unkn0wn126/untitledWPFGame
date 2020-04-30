using System;

namespace Engine.Models.Components.Navmesh
{
    /// <summary>
    /// Represents the directions
    /// in which the given navmesh
    /// borders with another navmesh
    /// </summary>
    [Flags]
    public enum NavmeshContinues
    {
        None = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
    }

    /// <summary>
    /// Component used to keep track of
    /// possible walkable areas for the NPCS
    /// </summary>
    public interface INavmeshComponent : IGameComponent
    {
        public bool LeadsDown { get; set; }
        public bool LeadsUp { get; set; }
        public bool LeadsLeft { get; set; }
        public bool LeadsRight { get; set; }
    }
}
