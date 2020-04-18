using Engine.Models.Components.RigidBody;
using Engine.Models.Scenes;
using Engine.ViewModels;
using GameInputHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TimeUtils;

namespace Engine.Models.Components.Script
{
    public class PlayerMovementScript : IScriptComponent
    {
        private float _baseVelocity;
        private uint _player;

        private float _baseForceX;
        private float _baseForceY;

        private IScene _context;

        private GameInput _gameInputHandler;

        private GameTime _gameTime;

        public PlayerMovementScript(GameTime gameTime, GameInput gameInputHandler, IScene context, uint player, float baseVelocity)
        {
            _gameTime = gameTime;
            _gameInputHandler = gameInputHandler;
            _baseVelocity = baseVelocity;
            _player = player;
            _context = context;

            _baseForceX = 0;
            _baseForceY = 0;
        }

        public void Update()
        {
            GameKey currValue = _gameInputHandler.CurrentKeyValue;
            IRigidBodyComponent rigidBody = _context.EntityManager.GetRigidBodyComponent(_player);

            if ((currValue & GameKey.Up) == GameKey.Up)
                rigidBody.ForceY = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Down) == GameKey.Down)
                rigidBody.ForceY = +_baseVelocity * _gameTime.DeltaTimeInSeconds;            
            if ((currValue & GameKey.Left) == GameKey.Left)
                rigidBody.ForceX = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Right) == GameKey.Right)
                rigidBody.ForceX = +_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Up) != GameKey.Up && (currValue & GameKey.Down) != GameKey.Down)
                rigidBody.ForceY = _baseForceY * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Left) != GameKey.Left && (currValue & GameKey.Right) != GameKey.Right)
                rigidBody.ForceX = _baseForceX * _gameTime.DeltaTimeInSeconds;
        }
    }
}
