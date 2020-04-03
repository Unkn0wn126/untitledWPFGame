#define TRACE
using Engine.Coordinates;
using Engine.Models.Components;
using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Engine.Models.Cameras
{
    public class Camera : ICamera
    {
        private float _halfWidth;
        private float _halfHeight;
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public List<IGraphicsComponent> VisibleObjects { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }

        // Keep window res as well as renderable res
        public Camera(float width, float height)
        {
            VisibleObjects = new List<IGraphicsComponent>();
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
        public void UpdatePosition(IGameObject focusPoint, IScene context)
        {
            ISpatialIndex grid = context.Grid;

            VisibleObjects = grid.GetGraphicsComponentsInRadius(focusPoint.Transform, 3);

            XOffset = _halfWidth - focusPoint.Transform.ScaleX;
            YOffset = _halfHeight - focusPoint.Transform.ScaleY;
        }
    }
}
