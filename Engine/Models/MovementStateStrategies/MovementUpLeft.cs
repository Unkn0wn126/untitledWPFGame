using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.MovementStateStrategies
{
    public class MovementUpLeft : IMovementStrategy
    {
        public void ExecuteStrategy(IGameObject entity)
        {
            float baseVelocity = 5f; // TODO: Change this to get the speed from the entity
            Vector2 newPos = entity.Transform.Position;
            newPos.X -= baseVelocity;
            newPos.Y -= baseVelocity;

            entity.Move(newPos);
        }
    }
}
