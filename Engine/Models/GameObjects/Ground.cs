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
    /// Object for representing a walkable ground
    /// </summary>
    public class Ground : IGameObject
    {
        private Vector2 _position;

        public Guid Id { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public IGraphicsComponent GraphicsComponent { get; set; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Grid Grid { get; set; }
        public IGameObject Prev { get; set; }
        public IGameObject Next { get; set; }

        public Ground(Grid grid, IGraphicsComponent graphicsComponent, Vector2 position, float width, float height)
        {
            Grid = grid;
            GraphicsComponent = graphicsComponent;
            Position = position;
            Width = width;
            Height = height;
            grid.Add(this);
        }

        public void Update(IScene logicContext)
        {
            GraphicsComponent.Update(this, logicContext);
        }

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            Grid.Move(this, Position.X, Position.Y);
        }
    }
}
