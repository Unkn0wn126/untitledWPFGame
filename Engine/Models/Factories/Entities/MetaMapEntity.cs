using Engine.Models.Components;
using Engine.Models.Components.Life;
using Engine.Models.Components.Navmesh;
using Engine.Models.Components.Script;
using ResourceManagers.Images;
using System;

namespace Engine.Models.Factories.Entities
{
    /// <summary>
    /// A blueprint for
    /// an actual entity
    /// with all the necessary
    /// information for generation
    /// </summary>
    [Serializable]
    public class MetaMapEntity
    {
        public ComponentState Components { get; set; }
        public ImgName Graphics { get; set; }
        public CollisionType CollisionType { get; set; }
        public NavmeshContinues NavmeshContinuation { get; set; }
        public ScriptType Scripts { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }
        public int ZIndex { get; set; }
        public ILifeComponent LifeComponent { get; set; }
    }
}
