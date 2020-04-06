#define TRACE
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

        public Vector2 Position { get => _position; set => _position = value; }
        public float Width { get; set; }
        public float Height { get; set; }
        public List<IGameObject> VisibleObjects { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }

        public Camera()
        {
            VisibleObjects = new List<IGameObject>();
            // This should be passed as a value in the future
            // gonna be based on the size of the window
            Width = 400;
            Height = 300;
            XOffset = 350;
            YOffset = 250;
        }

        /// <summary>
        /// Updates the list of objects visible by this camera
        /// </summary>
        /// <param name="focusPoint"></param>
        /// <param name="context"></param>
        public void UpdatePosition(IGameObject focusPoint, IScene context)
        {
            float halfWidth = Width / 2;
            float halfHeight = Height / 2;

            // visible on the screen to the left and to the right of the focus point
            float minX = focusPoint.Position.X - halfWidth;
            float maxX = focusPoint.Position.X + halfWidth;

            // visible on the screen up and down of the focus point
            float minY = focusPoint.Position.Y - halfHeight;
            float maxY = focusPoint.Position.Y + halfHeight;

            // Check boundary of x axis and then of y axis on reduced set of objects
            VisibleObjects = context.SceneElements.Where(x => x.Position.X + x.Width > minX && x.Position.X - x.Width < maxX && x != focusPoint)
                .Where(y => y.Position.Y + y.Height > minY && y.Position.Y - y.Height < maxY).ToList();
        }
    }
}
