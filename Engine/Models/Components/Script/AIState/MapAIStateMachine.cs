using Engine.Models.Components.RigidBody;
using Engine.Models.Scenes;
using System;
using TimeUtils;

namespace Engine.Models.Components.Script.AIState
{
    public enum MapAIState
    {
        Walking,
        Following
    }
    public class MapAIStateMachine
    {
        private float _baseVelocity;

        private float _baseForceX;
        private float _baseForceY;

        private GameTime _gameTime;

        private float _timer;

        private IScene _context;

        private Random _random;

        private int _direction;

        private int _waitTime;
        public uint Owner { get; set; }
        public uint Target { get; set; }
        private MapAIState _state;

        private IRigidBodyComponent _ownerRigidBody;
        private ICollisionComponent _ownerCollision;

        private ITransformComponent _ownerTransform;
        private ITransformComponent _targetTransform;

        public MapAIStateMachine(GameTime gameTime, IScene context, uint player, IRigidBodyComponent rigidBody, ITransformComponent ownerTransform, ICollisionComponent ownerCollision, float baseVelocity)
        {
            _context = context;
            _ownerRigidBody = rigidBody;
            _ownerTransform = ownerTransform;
            _ownerCollision = ownerCollision;
            _gameTime = gameTime;
            _baseVelocity = baseVelocity;

            Owner = player;

            _baseForceX = 0;
            _baseForceY = 0;

            _timer = 0;

            _random = new Random();
            _direction = _random.Next(6);
            _waitTime = _random.Next(10);

            _state = MapAIState.Walking;
        }

        public void SetStateToFollow(uint target, ITransformComponent targetTransform)
        {
            Target = target;
            _targetTransform = targetTransform;
            _state = MapAIState.Following;
        }

        public void SetStateToWalk()
        {
            _state = MapAIState.Walking;
        }

        public void ProcessState()
        {
            if (_state == MapAIState.Walking)
            {
                WalkAround();
            }
            else if (_state == MapAIState.Following)
            {
                FollowTarget();
            }
        }

        private void FollowTarget()
        {
            float diffX = _ownerTransform.Position.X - _targetTransform.Position.X;
            float diffY = _ownerTransform.Position.Y - _targetTransform.Position.Y;

            if (diffY > 0)
                _ownerRigidBody.ForceY = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if (diffY < 0)
                _ownerRigidBody.ForceY = +_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if (diffX > 0)
                _ownerRigidBody.ForceX = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if (diffX < 0)
                _ownerRigidBody.ForceX = +_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if (Math.Abs(diffY) <= 0)
                _ownerRigidBody.ForceY = _baseForceY * _gameTime.DeltaTimeInSeconds;
            if (Math.Abs(diffX) <= 0)
                _ownerRigidBody.ForceX = _baseForceX * _gameTime.DeltaTimeInSeconds;
        }

        private void WalkAround()
        {
            _timer += _gameTime.DeltaTimeInSeconds;

            if (_ownerCollision.CollidingWith.Count > 0)
            {
                foreach (var item in _ownerCollision.CollidingWith)
                {
                    ICollisionComponent current = _context.EntityManager.GetComponentOfType<ICollisionComponent>(item);
                    if (current.IsSolid && !current.IsDynamic)
                    {
                        int originalDirection = _direction;
                        while (_direction == originalDirection)
                        {
                            _direction = _random.Next(6);
                        }

                        _timer = 0;
                        _waitTime = _random.Next(10);
                    }
                }
            }

            if (_timer >= _waitTime)
            {
                _direction = _random.Next(6);
                _timer = 0;
                _waitTime = _random.Next(10);
            }

            _ownerRigidBody.ForceY = _baseForceY * _gameTime.DeltaTimeInSeconds;
            _ownerRigidBody.ForceX = _baseForceX * _gameTime.DeltaTimeInSeconds;

            if (_direction == 1)
                _ownerRigidBody.ForceY = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            else if (_direction == 2)
                _ownerRigidBody.ForceY = +_baseVelocity * _gameTime.DeltaTimeInSeconds;
            else if (_direction == 3)
                _ownerRigidBody.ForceX = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            else if (_direction == 4)
                _ownerRigidBody.ForceX = +_baseVelocity * _gameTime.DeltaTimeInSeconds;
        }
    }
}
