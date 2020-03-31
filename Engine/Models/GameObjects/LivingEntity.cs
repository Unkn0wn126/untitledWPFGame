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
        public Guid Id { get; set; }
        public ITransformComponent Transform { get; set; }
        public IGraphicsComponent GraphicsComponent { get; set; }
        public IGameComponent PlayerMovementComponent { get; set; }
        public Grid Grid { get; set; }

        public LivingEntity(Grid grid, IGraphicsComponent graphicsComponent, IGameComponent playerMovementComponent, ITransformComponent transform, float baseVelocity)
        {
            Grid = grid;
            GraphicsComponent = graphicsComponent;
            PlayerMovementComponent = playerMovementComponent;
            Transform = transform;
            grid.Add(this);
        }

        public void Update(IScene logicContext)
        {
            GraphicsComponent.Update(this, logicContext);
            PlayerMovementComponent.Update(this, logicContext);
        }

        public void Move(Vector2 newPos)
        {
            Transform.Position = newPos;
            Grid.Move(this, Transform.Position.X, Transform.Position.Y);
        }
    }
}
