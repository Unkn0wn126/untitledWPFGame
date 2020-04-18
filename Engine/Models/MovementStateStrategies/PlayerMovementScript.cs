using Engine.Models.Components.RigidBody;
using Engine.Models.Scenes;
using Engine.ViewModels;
using GameInputHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Engine.Models.MovementStateStrategies
{
    //public enum AxisStrategy
    //{
    //    UP,
    //    DOWN,
    //    LEFT,
    //    RIGHT,
    //    UPLEFT,
    //    UPRIGHT,
    //    DOWNLEFT,
    //    DOWNRIGHT,
    //    NEUTRAL
    //}
    public class PlayerMovementScript
    {
        private float _baseVelocity;
        private uint _player;

        private float _baseForceX;
        private float _baseForceY;

        private float _forceX;
        private float _forceY;

        private IScene _context;

        private GameInput _gameInputHandler;

        public PlayerMovementScript(GameInput gameInputHandler, IScene context, uint player, float baseVelocity)
        {
            _gameInputHandler = gameInputHandler;
            _baseVelocity = baseVelocity;
            _player = player;
            _context = context;

            _forceX = 0;
            _forceY = 0;

            _baseForceX = 0;
            _baseForceY = 0;
        }

        public void UpdatePosition()
        {
            switch (_gameInputHandler.CurrentKeyValue)
            {
                case GameKey.Up:
                    _forceX = _baseForceX;
                    _forceY = -_baseVelocity;
                    break;
                case GameKey.Down:
                    _forceX = _baseForceX;
                    _forceY = _baseVelocity;
                    break;
                case GameKey.Left:
                    _forceX = -_baseVelocity;
                    _forceY = _baseForceY;
                    break;
                case GameKey.Right:
                    _forceX = _baseVelocity;
                    _forceY = _baseForceY;
                    break;
                case GameKey.Up | GameKey.Left:
                    _forceX = -_baseVelocity;
                    _forceY = -_baseVelocity;
                    break;
                case GameKey.Up | GameKey.Right:
                    _forceX = _baseVelocity;
                    _forceY = -_baseVelocity;
                    break;
                case GameKey.Down | GameKey.Left:
                    _forceX = -_baseVelocity;
                    _forceY = _baseVelocity;
                    break;
                case GameKey.Down | GameKey.Right:
                    _forceX = _baseVelocity;
                    _forceY = _baseVelocity;
                    break;
                case GameKey.None:
                    _forceX = _baseForceX;
                    _forceY = _baseForceY;
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
