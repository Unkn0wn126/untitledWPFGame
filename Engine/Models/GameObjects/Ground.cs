﻿using Engine.Models.Components;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.GameObjects
{
    public class Ground : IGameObject
    {
        private Vector2 _position;
        private double _width;
        private double _height;
        private IGraphicsComponent _graphicsComponent;
        public double Width { get => _width; set => _width = value; }
        public double Height { get => _height; set => _height = value; }

        public IGraphicsComponent GraphicsComponent { get => _graphicsComponent; set => _graphicsComponent = value; }
        public Vector2 Position { get => _position; set => _position = value; }

        public Ground(IGraphicsComponent graphicsComponent, Vector2 position, double width, double height)
        {
            GraphicsComponent = graphicsComponent;
            Position = position;
            Width = width;
            Height = height;
        }

        public void Update(IScene logicContext)
        {
            GraphicsComponent.Update(this, logicContext);
        }
    }
}
