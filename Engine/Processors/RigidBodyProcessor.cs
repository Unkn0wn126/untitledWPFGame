using Engine.EntityManagers;
using Engine.Models.Components;
using Engine.Models.Components.RigidBody;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Processors
{
    public class RigidBodyProcessor : IProcessor
    {
        private IScene _context;
        public RigidBodyProcessor(IScene context)
        {
            _context = context;
        }
        public void ProcessOnEeGameTick(long lastFrameTime)
        {
            IEntityManager manager = _context.EntityManager;
            List<uint> active = manager.GetAllActiveEntities();

            List<IRigidBodyComponent> rigidBodies = new List<IRigidBodyComponent>();
            List<ITransformComponent> transforms = new List<ITransformComponent>();
            List<ICollisionComponent> collisions = new List<ICollisionComponent>();
            List<uint> useful = new List<uint>();

            active.ForEach(x =>
            {
                if (manager.EntityHasComponent(x, typeof(IRigidBodyComponent)) && manager.EntityHasComponent(x, typeof(ITransformComponent)) && manager.EntityHasComponent(x, typeof(ICollisionComponent)))
                {
                    rigidBodies.Add(manager.GetRigidBodyComponent(x));
                    transforms.Add(manager.GetTransformComponent(x));
                    collisions.Add(manager.GetCollisionComponent(x));
                    useful.Add(x);
                }
            });

            for (int i = 0; i < rigidBodies.Count; i++)
            {
                Vector2 newPos = UpdatePos(rigidBodies[i], transforms[i], lastFrameTime);
                collisions[i].CollidingWith.ForEach(x =>
                {
                    ICollisionComponent collision = manager.GetCollisionComponent(x);
                    if (!collision.IsDynamic && !collision.IsSolid)
                    {
                        ITransformComponent transform = manager.GetTransformComponent(x);
                        float diffX = transforms[i].Position.X - transform.Position.X;
                        float diffY = transforms[i].Position.Y - transform.Position.Y;
                        double deltaTime = GetDeltaTime(lastFrameTime);

                        // going in the direction of the collision
                        if (diffX < 0 && rigidBodies[i].ForceX > 0 || diffX > 0 && rigidBodies[i].ForceX < 0)
                        {
                            newPos.X -= rigidBodies[i].ForceX * (float) deltaTime;
                        }                        
                        if (diffY < 0 && rigidBodies[i].ForceY > 0 || diffY > 0 && rigidBodies[i].ForceY < 0)
                        {
                            newPos.Y -= rigidBodies[i].ForceY * (float)deltaTime;
                        }
                    }
                });
                manager.Coordinates.Move(useful[i], transforms[i], newPos.X, newPos.Y);
            }
        }

        private float GetDeltaTime(long lastFrameTime)
        {
            return (float)(lastFrameTime * 0.01);
        }

        private Vector2 UpdatePos(IRigidBodyComponent force, ITransformComponent transform, long lastFrameTime)
        {
            Vector2 newPos = transform.Position;
            double deltaTime = GetDeltaTime(lastFrameTime);
            newPos.X += force.ForceX * (float)deltaTime;
            newPos.Y += force.ForceY * (float)deltaTime;

            return newPos;
        }
    }
}
