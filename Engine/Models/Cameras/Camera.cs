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
        private Vector2 _position;
        private float _width;
        private float _height;
        private List<IGraphicsComponent> _visibleObjects;
        private float _xOffset;
        private float _yOffset;
        public Vector2 Position { get => _position; set => _position = value; }
        public float Width { get => _width; set => _width = value; }
        public float Height { get => _height; set => _height = value; }
        public List<IGraphicsComponent> VisibleObjects { get => _visibleObjects; set => _visibleObjects = value; }
        public float XOffset { get => _xOffset; set => _xOffset = value; }
        public float YOffset { get => _yOffset; set => _yOffset = value; }

        // Keep window res as well as renderable res
        public Camera(float width, float height)
        {
            VisibleObjects = new List<IGraphicsComponent>();
            // This should be passed as a value in the future
            // gonna be based on the size of the window
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Updates the list of objects visible by this camera
        /// </summary>
        /// <param name="focusPoint"></param>
        /// <param name="context"></param>
        public void UpdatePosition(IGraphicsComponent focusPoint, IScene context)
        {
            Grid grid = context.Grid;
            IGameObject focus = context.PlayerObject;

            VisibleObjects = grid.GetGraphicsComponentsInRadius(focus, 3);
            //temp.ForEach(x => VisibleObjects.Add(x.GraphicsComponent));
            float halfWidth = Width / 2;
            float halfHeight = Height / 2;

            XOffset = halfWidth - focusPoint.Transform.ScaleX;
            YOffset = halfHeight - focusPoint.Transform.ScaleY;
            // visible on the screen to the left and to the right of the focus point
            //    float minX = focusPoint.Position.X - halfWidth;
            //    float maxX = focusPoint.Position.X + halfWidth;

            //    // visible on the screen up and down of the focus point
            //    float minY = focusPoint.Position.Y - halfHeight;
            //    float maxY = focusPoint.Position.Y + halfHeight;

            //    VisibleObjects.Clear();
            //    // Check boundary of x axis and then of y axis on reduced set of objects
            //    //context.SceneGraphicsComponents.ForEach(x => { if (x.Position.X + x.Width > minX && x.Position.X - x.Width < maxX && x != focusPoint &&
            //    //    x.Position.Y + x.Height > minY && x.Position.Y - x.Height < maxY) { VisibleObjects.Add(x); }
            //    //});
            //    VisibleObjects = context.SceneGraphicsComponents.AsParallel().Where(x => x.Position.X + x.Width > minX && x.Position.X - x.Width < maxX && x != focusPoint)
            //        .Where(y => y.Position.Y + y.Height > minY && y.Position.Y - y.Height < maxY).ToList();
            //    //VisibleObjects = context.SceneElements.Where(x => x.Position.X + x.Width > minX && x.Position.X - x.Width < maxX && x != focusPoint)
            //    //    .Where(y => y.Position.Y + y.Height > minY && y.Position.Y - y.Height < maxY).ToList();
        }
    }
}
