using Engine.Models.Components.Life;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script.AIState;
using Engine.Models.Scenes;
using System;
using TimeUtils;

namespace Engine.Models.Components.Script
{
    public class AiMovementScript : IScriptComponent
    {
        private float _baseVelocity;
        private uint _npc;

        private float _baseForceX;
        private float _baseForceY;

        private IScene _context;

        private GameTime _gameTime;

        private float _timer;

        private Random _random;

        private int _direction;

        private MapAIStateMachine _state;

        public AiMovementScript(GameTime gameTime, IScene context, uint npc, float baseVelocity)
        {
            _gameTime = gameTime;
            _baseVelocity = baseVelocity;
            _npc = npc;
            _context = context;

            _baseForceX = 0;
            _baseForceY = 0;

            _timer = 0;

            _random = new Random();
            _direction = _random.Next(6);

            _state = new MapAIStateMachine(gameTime, _context, npc, _context.EntityManager.GetComponentOfType<IRigidBodyComponent>(_npc), _context.EntityManager.GetComponentOfType<ITransformComponent>(_npc), _context.EntityManager.GetComponentOfType<ICollisionComponent>(_npc), baseVelocity);
        }

        public void Update()
        {
            var active = _context.EntityManager.GetAllActiveEntities();
            var ownerTransform = _context.EntityManager.GetComponentOfType<ITransformComponent>(_npc);
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
