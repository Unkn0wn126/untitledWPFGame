using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using System.Collections.Generic;
using System.Numerics;

namespace Engine.Models.Cameras
{
    public interface ICamera
    {
        Vector2 Position { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        float XOffset { get; set; }
        float YOffset { get; set; }
        List<IGameObject> VisibleObjects { get; set; }
        void UpdatePosition(IGameObject focusPoint, IScene context);
    }
}
