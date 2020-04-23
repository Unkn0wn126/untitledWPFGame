using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using System;

namespace Engine.Models.Scenes
{
    /// <summary>
    /// A container for game objects
    /// basically a location with its own context
    /// </summary>
    public class GeneralScene : IScene
    {
        public IEntityManager EntityManager { get; set; }
        public ICamera SceneCamera { get; set; }
        public uint PlayerEntity { get; set; }
        public int NumOfObjectsInCell { get; set; }
        public int BaseObjectSize { get; set; }
        public int NumOfEntitiesOnX { get; set; }
        public int NumOfEntitiesOnY { get; set; }
        public ISpatialIndex Coordinates { get; set; }

        public GeneralScene(ICamera camera, IEntityManager entityManager, ISpatialIndex coordinates)
        {
            EntityManager = entityManager;
            SceneCamera = camera;
            Coordinates = coordinates;
        }
    }
}
