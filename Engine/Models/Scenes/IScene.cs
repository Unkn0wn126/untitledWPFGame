using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;

namespace Engine.Models.Scenes
{
    public interface IScene
    {
        uint PlayerEntity { get; set; }
        ISpatialIndex Coordinates { get; set; }
        ITransformComponent PlayerTransform { get; set; }
        IEntityManager EntityManager { get; set; }
        ICamera SceneCamera { get; set; }
    }
}
