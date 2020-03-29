using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models.Scenes
{
    /// <summary>
    /// A container for game objects
    /// basically a location with its own context
    /// </summary>
    public class GeneralScene : IScene
    {
        public List<IGameObject> SceneObjects { get; set; }
        public IGameObject PlayerObject { get; set; }
        public ICamera SceneCamera { get; set; }
        public List<IGraphicsComponent> SceneGraphicsComponents { get; set; }
        public IGraphicsComponent PlayerGraphicsComponent { get; set; }

        public GeneralScene(List<IGameObject> sceneElements, IGameObject playerObject, float visibleWidth, float visibleHeight)
        {
            SceneObjects = sceneElements;
            PlayerObject = playerObject;
            PlayerGraphicsComponent = PlayerObject.GraphicsComponent;
            SceneCamera = new Camera(visibleWidth, visibleHeight);
            SceneGraphicsComponents = new List<IGraphicsComponent>();
            SceneObjects.ForEach(x => SceneGraphicsComponents.Add(x.GraphicsComponent));
        }

        public void Update()
        {
            Parallel.ForEach(SceneObjects, item =>
            {
                item.Update(this);
            });
        }
    }
}
