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
    public class AiMovementScript : IScriptComponent
    {
        private float _baseVelocity;
        private uint _player;

        private float _baseForceX;
        private float _baseForceY;

        private IScene _context;

        private GameTime _gameTime;

        private float _timer;

        private Random _random;

        private int _direction;

        public AiMovementScript(GameTime gameTime, IScene context, uint player, float baseVelocity)
        {
            _gameTime = gameTime;
            _baseVelocity = baseVelocity;
            _player = player;
            _context = context;

            _baseForceX = 0;
            _baseForceY = 0;

            _timer = 0;

            _random = new Random();
            _direction = _random.Next(6);
        }

        public void Update()
        {
            _timer += _gameTime.DeltaTimeInSeconds;

            IRigidBodyComponent rigidBody = _context.EntityManager.GetComponentOfType<IRigidBodyComponent>(_player);

            if (_timer >= 2)
            {
                _direction = _random.Next(6);
                _timer = 0;
            }

            rigidBody.ForceY = _baseForceY * _gameTime.DeltaTimeInSeconds;
            rigidBody.ForceX = _baseForceX * _gameTime.DeltaTimeInSeconds;

            if (_direction == 1)
                rigidBody.ForceY = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            else if (_direction == 2)
                rigidBody.ForceY = +_baseVelocity * _gameTime.DeltaTimeInSeconds;            
            else if (_direction == 3)
                rigidBody.ForceX = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            else if (_direction == 4)
                rigidBody.ForceX = +_baseVelocity * _gameTime.DeltaTimeInSeconds; 
        }
    }
}
