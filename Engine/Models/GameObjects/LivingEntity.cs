using Engine.Models.Components;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.GameObjects
{
    public class LivingEntity : IGameObject
    {
        private Vector2 _position;
        private double _width;
        private double _height;
        private IGraphicsComponent _graphicsComponent;
        public double Width { get => _width; set => _width = value; }
        public double Height { get => _height; set => _height = value; }

        public IGraphicsComponent GraphicsComponent { get => _graphicsComponent; set => _graphicsComponent = value; }
        public IGameComponent PlayerMovementComponent { get; set; }
        public Vector2 Position { get => _position; set => _position = value; }

        public LivingEntity(IGraphicsComponent graphicsComponent, IGameComponent playerMovementComponent, double width, double height, Vector2 initialPosition, float baseVelocity)
        {
            GraphicsComponent = graphicsComponent;
            PlayerMovementComponent = playerMovementComponent;
            Height = width;
            Width = height;
            _position = initialPosition;
        }

        public void Update(IScene logicContext)
        {
            GraphicsComponent.Update(this, logicContext);
            PlayerMovementComponent.Update(this, logicContext);
        }
    }
}
