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
    public class LivingEntity : ILivingEntity
    {
        public Guid Id { get; set; }
        public ITransformComponent Transform { get; set; }
        public IGraphicsComponent GraphicsComponent { get; set; }
        public IEntityMovementComponent PlayerMovementComponent { get; set; }
        public IEntityStats Stats { get; set; }
        public ISpatialIndex Grid { get; set; }

        public LivingEntity(ISpatialIndex grid, IGraphicsComponent graphicsComponent, IEntityMovementComponent playerMovementComponent, ITransformComponent transform, IEntityStats stats)
        {
            Grid = grid;
            GraphicsComponent = graphicsComponent;
            PlayerMovementComponent = playerMovementComponent;
            Transform = transform;
            Stats = stats;
            grid.Add(this);
        }

        public void Update(IScene logicContext)
        {
            PlayerMovementComponent.Update(this);
        }

        public void Move(Vector2 newPos)
        {
            Transform.Position = newPos;
            Grid.Move(this, Transform.Position.X, Transform.Position.Y);
        }
    }
}
