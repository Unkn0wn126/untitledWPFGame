using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;

namespace Engine.Models.Scenes
{
    public interface IScene
    {
        public uint PlayerEntity { get; set; }
        public ISpatialIndex Coordinates { get; set; }
        public ITransformComponent Transform { get; set; }
        public IEntityManager EntityManager { get; set; }
        public ICamera SceneCamera { get; set; }
    }
}
