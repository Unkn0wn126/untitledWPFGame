using Engine.EntityManagers;
using Engine.Models.Components;
using Engine.Models.Components.RigidBody;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void ChangeContext(IScene context)
        {
            _context = context;
        }

        public void ProcessOneGameTick(float lastFrameTime)
        {
            IEntityManager manager = _context.EntityManager;
            List<uint> active = manager.GetAllActiveEntities();

            List<IRigidBodyComponent> rigidBodies = new List<IRigidBodyComponent>();
            List<ITransformComponent> transforms = new List<ITransformComponent>();
            List<ICollisionComponent> collisions = new List<ICollisionComponent>();
            List<uint> useful = new List<uint>();

            active.ForEach(x =>
            {
                if (manager.EntityHasComponent(x, typeof(IRigidBodyComponent)) && 
                manager.EntityHasComponent(x, typeof(ITransformComponent)) && 
                manager.EntityHasComponent(x, typeof(ICollisionComponent)))
                {
                    rigidBodies.Add(manager.GetRigidBodyComponent(x));
                    transforms.Add(manager.GetTransformComponent(x));
                    collisions.Add(manager.GetCollisionComponent(x));
                    useful.Add(x);
                }
            });

            for (int i = 0; i < rigidBodies.Count; i++)
            {
                Vector2 oldPos = transforms[i].Position;
                Vector2 newPos = UpdatePos(rigidBodies[i], oldPos, lastFrameTime);
                collisions[i].CollidingWith.ForEach(x =>
                {
                    ICollisionComponent collision = manager.GetCollisionComponent(x);
                    if (!collision.IsDynamic && !collision.IsSolid)
                    {
                        ITransformComponent transform = manager.GetTransformComponent(x);
                        // difference betveen origins on x
                        float originDiffX = oldPos.X - transform.Position.X;
                        // difference between origins on y
                        float originDiffY = oldPos.Y - transform.Position.Y;

                        int firstStartX = (int)Math.Round(oldPos.X);
                        int firstEndX = (int)Math.Round(oldPos.X + transforms[i].ScaleX);

                        int firstStartY = (int)Math.Round(oldPos.Y);
                        int firstEndY = (int)Math.Round(oldPos.Y + transforms[i].ScaleY);

                        int secondStartX = (int)Math.Round(transform.Position.X);
                        int secondEndX = (int)Math.Round(transform.Position.X + transform.ScaleX);

                        int secondStartY = (int)Math.Round(transform.Position.Y);
                        int secondEndY = (int)Math.Round(transform.Position.Y + transform.ScaleY);

                        // going in the direction of the collision
                        if ((originDiffX < 0 && rigidBodies[i].ForceX > 0 || originDiffX > 0 && rigidBodies[i].ForceX < 0)
                        && (firstStartY < secondEndY && firstEndY > secondStartY))
                        {
                            newPos.X = originDiffX < 0 ? secondStartX - transforms[i].ScaleX : secondEndX;
                        }
                        if ((originDiffY < 0 && rigidBodies[i].ForceY > 0 || originDiffY > 0 && rigidBodies[i].ForceY < 0)
                        && (firstStartX < secondEndX && firstEndX > secondStartX))
                        {
                            newPos.Y = originDiffY < 0 ? secondStartY - transforms[i].ScaleY : secondEndY;
                        }
                    }
                });

                transforms[i].Position = newPos;

                manager.Coordinates.Move(useful[i], oldPos, newPos);
            }
        }

        private Vector2 UpdatePos(IRigidBodyComponent force, Vector2 transform, float lastFrameTime)
        {
            Vector2 newPos = transform;
            //float deltaTime = GetDeltaTime(lastFrameTime);
            newPos.X += force.ForceX /** deltaTime*/;
            newPos.Y += force.ForceY /** deltaTime*/;

            return newPos;
        }
    }
}
