using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;

namespace Engine.Models.Scenes
{
    public interface IScene
    {
        uint PlayerEntity { get; set; }
        int NumOfObjectsInCell { get; set; }
        int BaseObjectSize { get; set; }
        int NumOfEntitiesOnX { get; set; }
        int NumOfEntitiesOnY { get; set; }
        ISpatialIndex Coordinates { get; set; }
        IEntityManager EntityManager { get; set; }
        ICamera SceneCamera { get; set; }
    }
}
