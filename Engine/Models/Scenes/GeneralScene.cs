using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

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

        public GeneralScene(List<IGameObject> sceneElements, IGameObject playerObject, float visibleWidth, float visibleHeight)
        {
            SceneElements = sceneElements;
            PlayerObject = playerObject;
            SceneCamera = new Camera(visibleWidth, visibleHeight);
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
