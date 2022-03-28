using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Factories.Scenes;

namespace Engine.Models.Scenes
{
    public delegate void SceneChange();

    /// <summary>
    /// A container for entity manager
    /// basically a location with its own context
    /// </summary>
    public interface IScene
    {
        SceneType SceneType { get; set; }
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
