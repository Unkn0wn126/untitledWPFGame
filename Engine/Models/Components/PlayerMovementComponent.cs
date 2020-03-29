//#define TRACE
using Engine.Models.GameObjects;
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
        private MovementState _currentState;

        public void SetMovementState(MovementState newState)
        {
            _currentState = newState;
        }

        public PlayerMovementComponent()
        {
            _currentState = MovementState.STILL;
        }

        /// <summary>
        /// Should be called on every tick
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logicContext"></param>
        public void Update(IGameObject entity, IScene logicContext)
        {
            float baseVelocity = 5f;
            Vector2 newPos = entity.Position;

                // basically a simple state machine for player movement
                switch (_currentState)
                {
                    case MovementState.UP:
                        newPos.Y -= baseVelocity;
                        entity.Position = newPos;
                        break;
                    case MovementState.DOWN:
                        newPos.Y += baseVelocity;
                        entity.Position = newPos;
                        break;
                    case MovementState.LEFT:
                        newPos.X -= baseVelocity;
                        entity.Position = newPos;
                        break;
                    case MovementState.RIGHT:
                        newPos.X += baseVelocity;
                        entity.Position = newPos;
                        break;
                    case MovementState.UPLEFT:
                        newPos.X -= baseVelocity;
                        newPos.Y -= baseVelocity;
                        entity.Position = newPos;
                        break;
                    case MovementState.UPRIGHT:
                        newPos.X += baseVelocity;
                        newPos.Y -= baseVelocity;
                        entity.Position = newPos;
                        break;
                    case MovementState.DOWNLEFT:
                        newPos.X -= baseVelocity;
                        newPos.Y += baseVelocity;
                        entity.Position = newPos;
                        break;
                    case MovementState.DOWNRIGHT:
                        newPos.X += baseVelocity;
                        newPos.Y += baseVelocity;
                        entity.Position = newPos;
                        break;
                    case MovementState.STILL:
                        break;
                }

                //Trace.WriteLine($"X: [{entity.Position.X}]; Y: [{entity.Position.Y}]");
        }
    }
}
