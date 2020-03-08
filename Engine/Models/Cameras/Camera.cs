#define TRACE
using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Engine.Models.Cameras
{
    public class Camera : ICamera
    {
        private Vector2 _position;
        private double _width;
        private double _height;
        private List<IGameObject> _visibleObjects;
        private float _xOffset;
        private float _yOffset;
        public Vector2 Position { get => _position; set => _position = value; }
        public double Width { get => _width; set => _width = value; }
        public double Height { get => _height; set => _height = value; }
        public List<IGameObject> VisibleObjects { get => _visibleObjects; set => _visibleObjects = value; }
        public float XOffset { get => _xOffset; set => _xOffset = value; }
        public float YOffset { get => _yOffset; set => _yOffset = value; }

        public Camera()
        {
            VisibleObjects = new List<IGameObject>();
            XOffset = 350;
            YOffset = 250;
        }

        public void UpdatePosition(IGameObject focusPoint, IScene context)
        {
            VisibleObjects = context.SceneElements.Where(x => Vector2.Distance(focusPoint.Position, x.Position) < 200 && x != focusPoint).ToList();
        }
    }
}
