using Engine.Coordinates;
using Engine.Models.Components;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.GameObjects
{
    /// <summary>
    /// Game object whose purpose is to move around
    /// the scene and to do so based on its AI or player input
    /// </summary>
    public class LivingEntity : IGameObject
    {
        private Vector2 _position;
        public Guid Id { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public IGameObject Prev { get; set; }
        public IGameObject Next { get; set; }
        public IGraphicsComponent GraphicsComponent { get; set; }
        public IGameComponent PlayerMovementComponent { get; set; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Grid Grid { get; set; }

        public LivingEntity(Grid grid, IGraphicsComponent graphicsComponent, IGameComponent playerMovementComponent, float width, float height, Vector2 initialPosition, float baseVelocity)
        {
            Grid = grid;
            GraphicsComponent = graphicsComponent;
            PlayerMovementComponent = playerMovementComponent;
            Height = width;
            Width = height;
            _position = initialPosition;
            grid.Add(this);
        }

        public void Update(IScene logicContext)
        {
            GraphicsComponent.Update(this, logicContext);
            PlayerMovementComponent.Update(this, logicContext);
        }

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            Grid.Move(this, Position.X, Position.Y);
        }
    }
}
