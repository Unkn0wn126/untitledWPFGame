using Engine.Models.Cameras;
using Engine.Models.GameObjects;
using System.Collections.Generic;

namespace Engine.Models.Scenes
{
    /// <summary>
    /// A container for game objects
    /// basically a location with its own context
    /// </summary>
    public class GeneralScene : IScene
    {
        public List<IGameObject> SceneElements { get; set; }
        public IGameObject PlayerObject { get; set; }
        public ICamera SceneCamera { get; set; }

        public GeneralScene(List<IGameObject> sceneElements, IGameObject playerObject)
        {
            SceneElements = sceneElements;
            PlayerObject = playerObject;
            SceneCamera = new Camera();
        }

        public void Update()
        {
            foreach (var item in SceneElements)
            {
                item.Update(this);
            }
        }
    }
}
