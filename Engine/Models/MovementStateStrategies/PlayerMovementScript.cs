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
            GameKey currValue = _gameInputHandler.CurrentKeyValue;
            if ((currValue & GameKey.Up) == GameKey.Up)
                _forceY = -_baseVelocity;
            if ((currValue & GameKey.Down) == GameKey.Down)
                _forceY = +_baseVelocity;            
            if ((currValue & GameKey.Left) == GameKey.Left)
                _forceX = -_baseVelocity;
            if ((currValue & GameKey.Right) == GameKey.Right)
                _forceX = +_baseVelocity;
            if ((currValue & GameKey.Up) != GameKey.Up && (currValue & GameKey.Down) != GameKey.Down)
                _forceY = _baseForceY;
            if ((currValue & GameKey.Left) != GameKey.Left && (currValue & GameKey.Right) != GameKey.Right)
                _forceX = _baseForceX;
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
