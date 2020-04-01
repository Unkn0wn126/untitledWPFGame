//#define TRACE
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

    public class PlayerMovementComponent : IGameComponent
    {
        private IMovementStrategy _currentMovementStrategy;

        public void SetMovementState(IMovementStrategy newMovementStrategy)
        {
            _currentMovementStrategy = newMovementStrategy;
        }

        public PlayerMovementComponent()
        {
            _currentMovementStrategy = null;
        }

        /// <summary>
        /// Should be called on every tick
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logicContext"></param>
        public void Update(IGameObject entity, IScene logicContext)
        {
            _currentMovementStrategy?.ExecuteStrategy(entity);
        }
    }
}
