using Engine.Models.Components.Life;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script.AIState;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using TimeUtils;

namespace Engine.Models.Components.Script
{
    /// <summary>
    /// Script component used to change
    /// the ai movement state on the map
    /// </summary>
    public class AiMovementScript : IScriptComponent
    {
        private readonly uint _npc;

        private readonly IScene _context;

        private readonly MapAIStateMachine _state;

        public AiMovementScript(GameTime gameTime, IScene context, uint npc, 
            float baseVelocity, BattleInitialization battleInitialize)
        {
            _npc = npc;
            _context = context;

            _state = new MapAIStateMachine(gameTime, _context, npc, 
                _context.EntityManager.GetComponentOfType<IRigidBodyComponent>(_npc), 
                _context.EntityManager.GetComponentOfType<ITransformComponent>(_npc), 
                _context.EntityManager.GetComponentOfType<ICollisionComponent>(_npc), 
                baseVelocity, battleInitialize);
        }

        public void Update()
        {
            List<uint> active = _context.EntityManager.GetAllActiveEntities();
            ITransformComponent ownerTransform = _context.EntityManager.GetComponentOfType<ITransformComponent>(_npc);
            _state.SetStateToWalk();
            foreach (uint item in active)
            {
                if (_context.EntityManager.EntityHasComponent<ITransformComponent>(item) && 
                    _context.EntityManager.EntityHasComponent<ILifeComponent>(item))
                {
                    ITransformComponent transform = _context.EntityManager.GetComponentOfType<ITransformComponent>(item);
                    ILifeComponent life = _context.EntityManager.GetComponentOfType<ILifeComponent>(item);
                    if (IsDistanceLowerThan(transform.Position.X, ownerTransform.Position.X, 3) && 
                        IsDistanceLowerThan(transform.Position.Y, ownerTransform.Position.Y, 3) && life.IsPlayer)
                    {
                        _state.SetStateToFollow(item, transform);
                        break;
                    }
                }
            }
            _state.ProcessState();

        }

        /// <summary>
        /// Checks if the distance between
        /// the owner entity and the other
        /// entity is lower than a given value
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private bool IsDistanceLowerThan(float firstPoint, float secondPoint, int distance)
        {
            return Math.Abs(firstPoint - secondPoint) <= distance;
        }
    }
}
