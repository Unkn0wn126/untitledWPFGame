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
                        // difference betveen origins on x
                        float originDiffX = transforms[i].Position.X - transform.Position.X;
                        // difference between origins on y
                        float originDiffY = transforms[i].Position.Y - transform.Position.Y;

                        int firstStartX = (int)Math.Round(transforms[i].Position.X);
                        int firstEndX = (int)Math.Round(transforms[i].Position.X + transforms[i].ScaleX);

                        int firstStartY = (int)Math.Round(transforms[i].Position.Y);
                        int firstEndY = (int)Math.Round(transforms[i].Position.Y + transforms[i].ScaleY);

                        int secondStartX = (int)Math.Round(transform.Position.X);
                        int secondEndX = (int)Math.Round(transform.Position.X + transform.ScaleX);

                        int secondStartY = (int)Math.Round(transform.Position.Y);
                        int secondEndY = (int)Math.Round(transform.Position.Y + transform.ScaleY);

                        float deltaTime = GetDeltaTime(lastFrameTime);

                        //// going in the direction of the collision
                        if ((originDiffX < 0 && rigidBodies[i].ForceX > 0 || originDiffX > 0 && rigidBodies[i].ForceX < 0)
                        && (firstStartY < secondEndY && firstEndY > secondStartY))
                        {
                            newPos.X -= rigidBodies[i].ForceX * deltaTime;
                        }
                        if ((originDiffY < 0 && rigidBodies[i].ForceY > 0 || originDiffY > 0 && rigidBodies[i].ForceY < 0)
                        && (firstStartX < secondEndX && firstEndX > secondStartX))
                        {
                            newPos.Y -= rigidBodies[i].ForceY * deltaTime;
                        }
                    }
                });
                manager.Coordinates.Move(useful[i], transforms[i], newPos.X, newPos.Y);
            }
        }

        private float GetDeltaTime(long lastFrameTime)
        {
            return (lastFrameTime / 10000000f);
        }

        private Vector2 UpdatePos(IRigidBodyComponent force, ITransformComponent transform, long lastFrameTime)
        {
            Vector2 newPos = transform.Position;
            float deltaTime = GetDeltaTime(lastFrameTime);
            newPos.X += force.ForceX * deltaTime;
            newPos.Y += force.ForceY * deltaTime;

            return newPos;
        }
    }
}
