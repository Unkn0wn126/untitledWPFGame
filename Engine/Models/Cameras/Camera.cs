using Engine.EntityManagers;
using Engine.Models.Components;
using System.Collections.Generic;

namespace Engine.Models.Cameras
{
    public class Camera : ICamera
    {
        private float _halfWidth;
        private float _halfHeight;
        public float Width { get; set; }
        public float Height { get; set; }
        public List<IGraphicsComponent> VisibleObjects { get; set; }
        public List<ITransformComponent> VisibleTransforms { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }

        // Keep window res as well as renderable res
        public Camera(float width, float height)
        {
            VisibleObjects = new List<IGraphicsComponent>();
            VisibleTransforms = new List<ITransformComponent>();
            // This should be passed as a value in the future
            // gonna be based on the size of the window
            Width = width;
            Height = height;
            _halfWidth = Width / 2;
            _halfHeight = Height / 2;
        }

        /// <summary>
        /// Updates the list of objects visible by this camera
        /// </summary>
        /// <param name="focusPoint"></param>
        /// <param name="context"></param>
        public void UpdatePosition(ITransformComponent focusPoint, List<IGraphicsComponent> graphicsComponents, List<ITransformComponent> transformComponents)
        {
            VisibleObjects = graphicsComponents;
            VisibleTransforms = transformComponents;

            XOffset = _halfWidth - focusPoint.ScaleX;
            YOffset = _halfHeight - focusPoint.ScaleY;
        }
    }
}
