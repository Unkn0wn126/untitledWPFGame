using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Scenes
{
    public interface IScene
    {
        public List<IGraphicsComponent> SceneGraphicsComponents { get; set; }
        public List<IGameObject> SceneObjects { get; set; }
        public IGameObject PlayerObject { get; set; }
        public IGraphicsComponent PlayerGraphicsComponent { get; set; }
        public ICamera SceneCamera { get; set; }
        public void Update();
    }
}
