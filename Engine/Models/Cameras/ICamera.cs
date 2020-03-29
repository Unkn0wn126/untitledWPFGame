using Engine.Models.Components;
using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.Cameras
{
    public interface ICamera
    {
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public List<IGraphicsComponent> VisibleObjects { get; set; }
        public void UpdatePosition(IGraphicsComponent focusPoint, IScene context);
    }
}
