using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Factories.Scenes;

namespace Engine.Models.Scenes
{
    public class GeneralScene : IScene
    {
        public SceneType SceneType { get; set; }
        public IEntityManager EntityManager { get; set; }
        public ICamera SceneCamera { get; set; }
        public uint PlayerEntity { get; set; }
        public int NumOfObjectsInCell { get; set; }
        public int BaseObjectSize { get; set; }
        public int NumOfEntitiesOnX { get; set; }
        public int NumOfEntitiesOnY { get; set; }
        public ISpatialIndex Coordinates { get; set; }

        public GeneralScene(ICamera camera, IEntityManager entityManager, ISpatialIndex coordinates, SceneType sceneType)
        {
            SceneType = sceneType;
            EntityManager = entityManager;
            SceneCamera = camera;
            Coordinates = coordinates;
        }

    }
}
