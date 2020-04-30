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
        private float _sizeMultiplier;

        public float Width { get; set; }
        public float Height { get; set; }
        public List<IGraphicsComponent> VisibleObjects { get; set; }
        public List<ITransformComponent> VisibleTransforms { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public ITransformComponent FocusPoint { get; set; }


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

        public void UpdateFocusPoint(ITransformComponent focusPoint)
        {
            FocusPoint = focusPoint;
        }

        public void UpdatePosition(Dictionary<ITransformComponent, IGraphicsComponent> renderables)
        {
            Dictionary<ITransformComponent, IGraphicsComponent> keyValuePairs = 
                renderables.OrderBy(x => x.Key.ZIndex).ToDictionary(x => x.Key, x => x.Value);

            VisibleObjects = keyValuePairs.Values.ToList();
            VisibleTransforms = keyValuePairs.Keys.ToList();

            XOffset = Math.Abs(_halfWidth - ((FocusPoint.ScaleX * _sizeMultiplier) / 2f));
            YOffset = Math.Abs(_halfHeight - ((FocusPoint.ScaleY * _sizeMultiplier) / 2f));
        }
    }
}
