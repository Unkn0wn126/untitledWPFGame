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
    public class FollowPlayerScript : IScriptComponent
    {
        private float _baseVelocity;
        private uint _player;
        private uint _target;

        private float _baseForceX;
        private float _baseForceY;

        private IScene _context;

        private GameTime _gameTime;

        public FollowPlayerScript(GameTime gameTime, IScene context, uint player, uint target, float baseVelocity)
        {
            _gameTime = gameTime;
            _baseVelocity = baseVelocity;
            _player = player;
            _target = target;
            _context = context;

            _baseForceX = 0;
            _baseForceY = 0;
        }

        public void Update()
        {
            IRigidBodyComponent rigidBody = _context.EntityManager.GetComponentOfType<IRigidBodyComponent>(_player);
            ITransformComponent currPos = _context.EntityManager.GetComponentOfType<ITransformComponent>(_player);
            ITransformComponent targetPos = _context.EntityManager.GetComponentOfType<ITransformComponent>(_target);

            float diffX = currPos.Position.X - targetPos.Position.X;
            float diffY = currPos.Position.Y - targetPos.Position.Y;

            if (diffY > targetPos.ScaleY)
                rigidBody.ForceY = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if (diffY < targetPos.ScaleY)
                rigidBody.ForceY = +_baseVelocity * _gameTime.DeltaTimeInSeconds;            
            if (diffX > targetPos.ScaleX)
                rigidBody.ForceX = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if (diffX < targetPos.ScaleX)
                rigidBody.ForceX = +_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if (Math.Abs(diffY) < targetPos.ScaleY)
                rigidBody.ForceY = _baseForceY * _gameTime.DeltaTimeInSeconds;
            if (Math.Abs(diffX) < targetPos.ScaleX)
                rigidBody.ForceX = _baseForceX * _gameTime.DeltaTimeInSeconds;
        }
    }
}
