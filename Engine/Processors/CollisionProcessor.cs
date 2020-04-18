using Engine.EntityManagers;
using Engine.Models.Components;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Engine.Processors
{
    public class CollisionProcessor : IProcessor
    {

        private IScene _context;

        public CollisionProcessor(IScene context)
        {
            _context = context;
        }

        public void ProcessOneGameTick(float lastFrameTime)
        {
            List<ICollisionComponent> collisions = new List<ICollisionComponent>();
            List<ITransformComponent> transforms = new List<ITransformComponent>();
            List<uint> active = _context.EntityManager.GetAllActiveEntities();
            List<uint> useful = new List<uint>();
            active.ForEach(x =>
            {
                if (_context.EntityManager.EntityHasComponent(x, typeof(ICollisionComponent)) && _context.EntityManager.EntityHasComponent(x, typeof(ITransformComponent)))
                {
                    var a = _context.EntityManager.GetCollisionComponent(x);
                    var b = _context.EntityManager.GetTransformComponent(x);

                    if (!collisions.Contains(a) && !transforms.Contains(b))
                    {
                        collisions.Add(a);
                        transforms.Add(b);
                        useful.Add(x);
                    }
                        
                }
            });

            for (int i = 0; i < collisions.Count; i++)
            {
                for (int j = 0; j < collisions.Count; j++)
                {
                    //Trace.WriteLine($"j: {j}; i: {i}; count: {collisions.Count}");
                    if (IsPairColliding(transforms[i], transforms[j]) && transforms[i] != transforms[j])
                    {
                        if (!collisions[i].CollidingWith.Contains(useful[j]) && collisions[i].IsDynamic)
                        {
                            collisions[i].CollidingWith.Add(useful[j]);
                        }
                        
                        if (!collisions[j].CollidingWith.Contains(useful[i]) && collisions[j].IsDynamic)
                        {
                            collisions[j].CollidingWith.Add(useful[i]);
                        }
                    }
                    else
                    {
                        if (collisions[i].CollidingWith.Contains(useful[j]) && collisions[i].IsDynamic)
                        {
                            collisions[i].CollidingWith.Remove(useful[j]);
                        }

                        if (collisions[j].CollidingWith.Contains(useful[i]) && collisions[j].IsDynamic)
                        {
                            collisions[j].CollidingWith.Remove(useful[i]);
                        }

                    }
                }
            }

            //foreach (var item in collisions)
            //{
            //    if (item.IsDynamic && item.CollidingWith.Count > 0)
            //    {
            //        Trace.WriteLine("Colliding!");
            //    }
            //    else if (item.IsDynamic && item.CollidingWith.Count == 0)
            //    {
            //        Trace.WriteLine("Not Colliding!");
            //    }
            //}
        }

        private bool IsPairColliding(ITransformComponent firstItem, ITransformComponent secondItem)
        {
            int itemOneMinX = (int)Math.Round(firstItem.Position.X);
            int itemOneMaxX = (int)Math.Round(itemOneMinX + firstItem.ScaleX);

            int itemOneMinY = (int)Math.Round(firstItem.Position.Y);
            int itemOneMaxY = (int)Math.Round(itemOneMinY + firstItem.ScaleY);

            int itemTwoMinX = (int)Math.Round(secondItem.Position.X);
            int itemTwoMaxX = (int)Math.Round(itemTwoMinX + secondItem.ScaleX);

            int itemTwoMinY = (int)Math.Round(secondItem.Position.Y);
            int itemTwoMaxY = (int)Math.Round(itemTwoMinY + secondItem.ScaleY);

            return (IsBetween(itemOneMinX, itemOneMaxX, itemTwoMinX) || IsBetween(itemOneMinX, itemOneMaxX, itemTwoMaxX)) && (IsBetween(itemOneMinY, itemOneMaxY, itemTwoMinY) || IsBetween(itemOneMinY, itemOneMaxY, itemTwoMaxY));
        }

        private bool IsBetween(int minBound, int maxBound, int tested)
        {
            return tested >= minBound && tested <= maxBound;
        }
    }
}
