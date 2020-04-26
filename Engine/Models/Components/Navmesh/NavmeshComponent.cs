namespace Engine.Models.Components.Navmesh
{
    public class NavmeshComponent : INavmeshComponent
    {
        public bool LeadsDown { get; set; }
        public bool LeadsUp { get; set; }
        public bool LeadsLeft { get; set; }
        public bool LeadsRight { get; set; }
    }
}
