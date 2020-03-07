using Engine.Models.Cameras;
using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

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
