using Engine.EntityManagers;
using Engine.Models.Components;
using System.Collections.Generic;

namespace Engine.Models.Cameras
{
    /// <summary>
    /// Serves as a mediator between
    /// logical positions and
    /// the graphics engine
    /// </summary>
    public interface ICamera
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public ITransformComponent FocusPoint { get; set; }
        public List<IGraphicsComponent> VisibleObjects { get; set; }
        public List<ITransformComponent> VisibleTransforms { get; set; }

        /// <summary>
        /// Updates the size information
        /// for this camera.
        /// Used to keep 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void UpdateSize(float width, float height);

        /// <summary>
        /// Updates the point that
        /// is in the middle of the rendered window
        /// </summary>
        /// <param name="focusPoint"></param>
        public void UpdateFocusPoint(ITransformComponent focusPoint);

        /// <summary>
        /// Updates the position of a point that
        /// is in the middle of the rendered window.
        /// Based on this position also updates visible
        /// entities and their positions.
        /// </summary>
        /// <param name="renderables"></param>
        public void UpdatePosition(Dictionary<ITransformComponent, IGraphicsComponent> renderables);
    }
}
