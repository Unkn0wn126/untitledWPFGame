using Engine.EntityManagers;
using Engine.Models.Components;
using System.Collections.Generic;

namespace Engine.Models.Cameras
{
    public interface ICamera
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public ITransformComponent FocusPoint { get; set; }
        public List<IGraphicsComponent> VisibleObjects { get; set; }
        public List<ITransformComponent> VisibleTransforms { get; set; }
        void UpdateSize(float width, float height);
        public void UpdateFocusPoint(ITransformComponent focusPoint);
        public void UpdatePosition(Dictionary<ITransformComponent, IGraphicsComponent> renderables);
    }
}
