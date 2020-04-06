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
        private List<IGameObject> _sceneElements;
        private IGameObject _playerObject;
        private ICamera _sceneCamera;
        public List<IGameObject> SceneElements { get => _sceneElements; set => _sceneElements = value; }
        public IGameObject PlayerObject { get => _playerObject; set => _playerObject = value; }
        public ICamera SceneCamera { get => _sceneCamera; set => _sceneCamera = value; }

        public GeneralScene(List<IGameObject> sceneElements, IGameObject playerObject)
        {
            SceneElements = sceneElements;
            PlayerObject = playerObject;
            SceneCamera = new Camera();
        }

        public void Update()
        {
            foreach (var item in _sceneElements)
            {
                item.Update(this);
            }
        }
    }
}
