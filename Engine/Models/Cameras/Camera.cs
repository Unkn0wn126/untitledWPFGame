using Engine.EntityManagers;
using Engine.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private float _sizeMultiplier;

        // Keep window res as well as renderable res
        public Camera(float width, float height)
        {
            VisibleObjects = new List<IGraphicsComponent>();
            VisibleTransforms = new List<ITransformComponent>();

            UpdateSize(width, height);
        }

        public void UpdateSize(float width, float height)
        {
            Width = width;
            Height = height;
            _halfWidth = Width / 2;
            _halfHeight = Height / 2;
            _sizeMultiplier = (int)Math.Ceiling(width / 16f);
        }

        /// <summary>
        /// Updates the list of objects visible by this camera
        /// </summary>
        /// <param name="focusPoint"></param>
        /// <param name="context"></param>
        public void UpdatePosition(ITransformComponent focusPoint, Dictionary<ITransformComponent, IGraphicsComponent> renderables)
        {
            //VisibleObjects = graphicsComponents;
            //VisibleTransforms = transformComponents;

            Dictionary<ITransformComponent, IGraphicsComponent> keyValuePairs = renderables.OrderBy(x => x.Key.ZIndex).ToDictionary(x => x.Key, x => x.Value);

            //keyValuePairs.OrderBy(x => x.Key.ZIndex);

            VisibleObjects = keyValuePairs.Values.ToList();
            VisibleTransforms = keyValuePairs.Keys.ToList();

            XOffset = Math.Abs(_halfWidth - ((focusPoint.ScaleX * _sizeMultiplier) / 2f));
            YOffset = Math.Abs(_halfHeight - ((focusPoint.ScaleY * _sizeMultiplier) / 2f));
        }
    }
}
