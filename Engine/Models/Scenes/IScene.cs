using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using System;

namespace Engine.Models.Scenes
{
    public interface IScene
    {
        uint PlayerEntity { get; set; }
        Guid SceneID { get; set; }
        Guid NextScene { get; set; }
        int NumOfObjectsInCell { get; set; }
        int BaseObjectSize { get; set; }
        int NumOfEntitiesOnX { get; set; }
        int NumOfEntitiesOnY { get; set; }
        ISpatialIndex Coordinates { get; set; }
        ITransformComponent PlayerTransform { get; set; }
        IEntityManager EntityManager { get; set; }
        ICamera SceneCamera { get; set; }
    }
}
