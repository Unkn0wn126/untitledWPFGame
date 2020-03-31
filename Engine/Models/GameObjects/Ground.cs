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
        public Guid Id { get; set; }

        public IGraphicsComponent GraphicsComponent { get; set; }
        public ITransformComponent Transform { get; set; }
        public Grid Grid { get; set; }

        public Ground(Grid grid, IGraphicsComponent graphicsComponent, ITransformComponent transform)
        {
            Grid = grid;
            GraphicsComponent = graphicsComponent;
            Transform = transform;
            grid.Add(this);
        }

        public void Update(IScene logicContext)
        {
            GraphicsComponent.Update(this, logicContext);
        }

        public void Move(Vector2 newPos)
        {
            Transform.Position = newPos;
            Grid.Move(this, Transform.Position.X, Transform.Position.Y);
        }
    }
}
