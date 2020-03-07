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
        private float _lastPosX;
        private float _lastPosY;
        public Vector2 Position { get => _position; set => _position = value; }
        public double Width { get => _width; set => _width = value; }
        public double Height { get => _height; set => _height = value; }
        public List<IGameObject> VisibleObjects { get => _visibleObjects; set => _visibleObjects = value; }
        public float XOffset { get => _xOffset; set => _xOffset = value; }
        public float YOffset { get => _yOffset; set => _yOffset = value; }

        public Camera()
        {
            VisibleObjects = new List<IGameObject>();
        }

        public void UpdatePosition(IGameObject focusPoint, IScene context)
        {
            VisibleObjects.Clear();
            //Trace.WriteLine($"{focusPoint.Position.X}");
            XOffset = 350;
            YOffset = 250;

            Vector2 newLoc = focusPoint.Position;

            //if (focusPoint.Position.X <= 750 && focusPoint.Position.X >= 0)
            //{
            //    XOffset = focusPoint.Position.X;
            //}
            //else if (focusPoint.Position.X > 750)
            //{
            //    if (_lastPosX > focusPoint.Position.X)
            //    {
            //        XOffset -= 5f;
            //    }
            //    else
            //    {
            //        XOffset = XOffset < 750 ? XOffset += 5f : 750;
            //    }

            //}
            //else if (focusPoint.Position.X < 0)
            //{
            //    if (_lastPosX < focusPoint.Position.X)
            //    {
            //        XOffset += 5f;
            //    }
            //    else
            //    {
            //        XOffset = XOffset > 0 ? XOffset -= 5f : 0;
            //    }
            //}

            //if (focusPoint.Position.Y <= 550 && focusPoint.Position.Y >= 0)
            //{
            //    YOffset = focusPoint.Position.Y;
            //}
            //else if (focusPoint.Position.Y > 550)
            //{
            //    if (_lastPosY > focusPoint.Position.Y)
            //    {
            //        YOffset -= 5f;
            //    }
            //    else
            //    {
            //        YOffset = YOffset < 550 ? YOffset += 5f : 550;
            //    }
            //}
            //else if (focusPoint.Position.Y < 0)
            //{
            //    if (_lastPosY < focusPoint.Position.Y)
            //    {
            //        YOffset += 5f;
            //    }
            //    else
            //    {
            //        YOffset = YOffset > 0 ? YOffset -= 5f : 0;
            //    }
            //}

            _lastPosX = focusPoint.Position.X;
            _lastPosY = focusPoint.Position.Y;

            double borderXCoordLeft = focusPoint.Position.X - Width / 2;
            double borderXCoordRight = focusPoint.Position.X + Width / 2;

            VisibleObjects = context.SceneElements.Where(x => Vector2.Distance(focusPoint.Position, x.Position) < 800 && x != focusPoint).ToList();

            //foreach (var item in context.SceneElements)
            //{

            //    //Trace.WriteLine($"{Math.Abs(item.Position.X - focusPoint.Position.X)}");


            //    //XOffset = focusPoint.Position.X <= 800? focusPoint.Position.X : 400;
            //    //YOffset = focusPoint.Position.Y <= 300? focusPoint.Position.Y : 300;
            //    //Trace.WriteLine($"{Vector2.Distance(focusPoint.Position, item.Position)}");
            //    if (Vector2.Distance(focusPoint.Position, item.Position) < 300 && item != focusPoint)
            //    {
            //        VisibleObjects.Add(item);
            //    }

            //}

        }
    }
}
