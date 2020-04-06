using Engine.Models.Cameras;
using Engine.Models.GameObjects;
using System.Collections.Generic;

namespace Engine.Models.Scenes
{
    public interface IScene
    {
        public List<IGameObject> SceneElements { get; set; }
        public IGameObject PlayerObject { get; set; }
        public ICamera SceneCamera { get; set; }
        public void Update();
    }
}
