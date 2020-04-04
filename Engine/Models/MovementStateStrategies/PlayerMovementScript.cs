using Engine.Models.Components.RigidBody;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Engine.Models.MovementStateStrategies
{
    public enum AxisStrategy
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        UPLEFT,
        UPRIGHT,
        DOWNLEFT,
        DOWNRIGHT,
        NEUTRAL
    }
    public class PlayerMovementScript
    {
        private float _baseVelocity;
        private uint _player;

        private float _forceX;
        private float _forceY;

        private IScene _context;

        public PlayerMovementScript(IScene context, uint player, float baseVelocity)
        {
            _baseVelocity = baseVelocity;
            _player = player;
            _context = context;

            _forceX = 0;
            _forceY = 0;
        }

        public void UpdatePosition(AxisStrategy axisStrategy)
        {
            switch (axisStrategy)
            {
                case AxisStrategy.UP:
                    _forceX = 0;
                    _forceY = -_baseVelocity;
                    break;
                case AxisStrategy.DOWN:
                    _forceX = 0;
                    _forceY = _baseVelocity;
                    break;
                case AxisStrategy.LEFT:
                    _forceX = -_baseVelocity;
                    _forceY = 0;
                    break;
                case AxisStrategy.RIGHT:
                    _forceX = _baseVelocity;
                    _forceY = 0;
                    break;
                case AxisStrategy.UPLEFT:
                    _forceX = -_baseVelocity;
                    _forceY = -_baseVelocity;
                    break;
                case AxisStrategy.UPRIGHT:
                    _forceX = _baseVelocity;
                    _forceY = -_baseVelocity;
                    break;
                case AxisStrategy.DOWNLEFT:
                    _forceX = -_baseVelocity;
                    _forceY = _baseVelocity;
                    break;
                case AxisStrategy.DOWNRIGHT:
                    _forceX = _baseVelocity;
                    _forceY = _baseVelocity;
                    break;
                case AxisStrategy.NEUTRAL:
                    _forceX = 0;
                    _forceY = 0;
                    break;
            }
        }

        public void ApplyForce()
        {
                IRigidBodyComponent rigidBody = _context.EntityManager.GetRigidBodyComponent(_player);
                rigidBody.ForceX = _forceX;
                rigidBody.ForceY = _forceY;

            //Trace.WriteLine($"X: {rigidBody.ForceX}; Y: {rigidBody.ForceY = _forceY}");
        }
    }
}
