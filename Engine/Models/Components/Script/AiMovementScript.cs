using Engine.Models.Components.Life;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script.AIState;
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

        private MapAIStateMachine _state;

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

            _state = new MapAIStateMachine(gameTime, _context, player, _context.EntityManager.GetComponentOfType<IRigidBodyComponent>(_player), _context.EntityManager.GetComponentOfType<ITransformComponent>(_player), baseVelocity);
        }

        public void Update()
        {
            //_timer += _gameTime.DeltaTimeInSeconds;

            //IRigidBodyComponent rigidBody = _context.EntityManager.GetComponentOfType<IRigidBodyComponent>(_player);
            var active = _context.EntityManager.GetAllActiveEntities();
            var ownerTransform = _context.EntityManager.GetComponentOfType<ITransformComponent>(_player);
            _state.SetStateToWalk();
            foreach (var item in active)
            {
                if (_context.EntityManager.EntityHasComponent<ITransformComponent>(item) && _context.EntityManager.EntityHasComponent<ILifeComponent>(item))
                {
                    var transform = _context.EntityManager.GetComponentOfType<ITransformComponent>(item);
                    var life = _context.EntityManager.GetComponentOfType<ILifeComponent>(item);
                    if (IsDistanceLowerThan(transform.Position.X, ownerTransform.Position.X, 3) && IsDistanceLowerThan(transform.Position.Y, ownerTransform.Position.Y, 3) && life.IsPlayer)
                    {
                        _state.SetStateToFollow(item, transform);
                        break;
                    }
                }
            }
            _state.ProcessState();

        }

        private bool IsDistanceLowerThan(float firstPoint, float secondPoint, int distance)
        {
            return Math.Abs(firstPoint - secondPoint) <= distance;
        }
    }
}
