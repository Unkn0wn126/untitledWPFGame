using Engine.Models.Cameras;
using Engine.Models.GameObjects;
using System.Collections.Generic;

namespace Engine.Models.Scenes
{
    public interface IScene
    {
        List<IGameObject> SceneElements { get; set; }
        IGameObject PlayerObject { get; set; }
        ICamera SceneCamera { get; set; }
        void Update();
    }
}
