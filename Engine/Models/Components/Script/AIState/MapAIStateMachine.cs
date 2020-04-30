using Engine.Models.Components.RigidBody;
using Engine.Models.Scenes;
using System;
using TimeUtils;

namespace Engine.Models.Components.Script.AIState
{
    /// <summary>
    /// Used to represent the
    /// states of the AI on
    /// the world map
    /// </summary>
    public enum MapAIState
    {
        Walking,
        Following
    }

    /// <summary>
    /// Used to process the desired
    /// map AI behavior
    /// </summary>
    public class MapAIStateMachine
    {
        private readonly float _baseVelocity;

        private readonly float _baseForceX;
        private readonly float _baseForceY;

        private readonly IScene _context;

        private readonly Random _random;

        private readonly IRigidBodyComponent _ownerRigidBody;
        private readonly ICollisionComponent _ownerCollision;

        private readonly ITransformComponent _ownerTransform;

        private readonly BattleInitialization _battleInitialize;

        private readonly GameTime _gameTime;

        private float _timer;

        private int _direction;

        private int _waitTime;

        private MapAIState _state;

        private ITransformComponent _targetTransform;

        public uint Owner { get; set; }
        public uint Target { get; set; }

        public MapAIStateMachine(GameTime gameTime, IScene context, uint player, IRigidBodyComponent rigidBody, 
            ITransformComponent ownerTransform, ICollisionComponent ownerCollision, float baseVelocity, 
            BattleInitialization battleInitialize)
        {
            _battleInitialize = battleInitialize;
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

        /// <summary>
        /// Sets the current state to "follow the entity"
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetTransform"></param>
        public void SetStateToFollow(uint target, ITransformComponent targetTransform)
        {
            Target = target;
            _targetTransform = targetTransform;
            _state = MapAIState.Following;
        }

        /// <summary>
        /// Sets the current state to "minding my own business"
        /// i. e. walk around
        /// </summary>
        public void SetStateToWalk()
        {
            _state = MapAIState.Walking;
        }

        /// <summary>
        /// Takes an action based on the
        /// current inner state
        /// </summary>
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

        /// <summary>
        /// Makes the owner entity move towards
        /// the target entity
        /// </summary>
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

            if (_ownerCollision.CollidingWith.Contains(Target))
            {
                _battleInitialize.Invoke(Owner);
            }
        }

        /// <summary>
        /// Recalculates the direction of
        /// the owner entity based on
        /// the current collisions
        /// </summary>
        private void RecalculateOnCollision()
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

        /// <summary>
        /// Randomly chooses a direction
        /// and then makes the entity go
        /// in that direction for a given
        /// period of time
        /// </summary>
        private void WalkAround()
        {
            _timer += _gameTime.DeltaTimeInSeconds;

            if (_ownerCollision.CollidingWith.Count > 0)
            {
                RecalculateOnCollision();
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
