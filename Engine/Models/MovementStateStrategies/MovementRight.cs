using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.MovementStateStrategies
{
    public class MovementRight : IMovementStrategy
    {
        public void ExecuteStrategy(ILivingEntity entity)
        {
            float baseVelocity = entity.Stats.Speed;
            Vector2 newPos = entity.Transform.Position;
            newPos.X += baseVelocity;

            entity.Move(newPos);
        }
    }
}
