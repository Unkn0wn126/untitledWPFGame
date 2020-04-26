using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;

namespace Engine.Models.Scenes
{
    public delegate void SceneChange();
    public interface IScene
    {
        event SceneChange SceneChange;
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
