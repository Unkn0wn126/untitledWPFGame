using Engine.Models.GameObjects;
using Engine.Models.MovementStateStrategies;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Engine.Models.Components
{

    public enum MovementState
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        UPLEFT,
        UPRIGHT,
        DOWNLEFT,
        DOWNRIGHT,
        STILL
    }

    public class EntityMovementComponent : IEntityMovementComponent
    {
        private IMovementStrategy _currentMovementStrategy;

        public void SetMovementState(IMovementStrategy newMovementStrategy)
        {
            _currentMovementStrategy = newMovementStrategy;
        }

        public EntityMovementComponent()
        {
            _currentMovementStrategy = null;
        }

        /// <summary>
        /// Should be called on every tick
        /// </summary>
        /// <param name="entity"></param>
        public void Update(ILivingEntity entity)
        {
            _currentMovementStrategy?.ExecuteStrategy(entity);
        }
    }
}
