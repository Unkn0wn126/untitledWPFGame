using Engine.Models.Components;
using Engine.Models.Scenes;
using System.Collections.Generic;

namespace Engine.Processors
{
    /// <summary>
    /// Handles collision calculations
    /// </summary>
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
            foreach (var x in active)
            {
                if (_context.EntityManager.EntityHasComponent<ICollisionComponent>(x) && _context.EntityManager.EntityHasComponent<ITransformComponent>(x))
                {
                    var a = _context.EntityManager.GetComponentOfType<ICollisionComponent>(x);
                    var b = _context.EntityManager.GetComponentOfType<ITransformComponent>(x);

                    if (!collisions.Contains(a) && !transforms.Contains(b))
                    {
                        collisions.Add(a);
                        transforms.Add(b);
                        useful.Add(x);
                    }

                }
            }

            for (int i = 0; i < collisions.Count; i++)
            {
                for (int j = 0; j < collisions.Count; j++)
                {
                    if (IsPairColliding(transforms[i], transforms[j]) && transforms[i] != transforms[j])
                    {
                        if (!collisions[i].CollidingWith.Contains(useful[j]))
                        {
                            collisions[i].CollidingWith.Add(useful[j]);
                        }
                        
                        if (!collisions[j].CollidingWith.Contains(useful[i]))
                        {
                            collisions[j].CollidingWith.Add(useful[i]);
                        }
                    }
                    else
                    {
                        if (collisions[i].CollidingWith.Contains(useful[j]))
                        {
                            collisions[i].CollidingWith.Remove(useful[j]);
                        }

                        if (collisions[j].CollidingWith.Contains(useful[i]))
                        {
                            collisions[j].CollidingWith.Remove(useful[i]);
                        }

                    }
                }
            }
        }

        private bool IsPairColliding(ITransformComponent firstItem, ITransformComponent secondItem)
        {
            float itemOneMinX = firstItem.Position.X;
            float itemOneMaxX = itemOneMinX + firstItem.ScaleX;

            float itemOneMinY = firstItem.Position.Y;
            float itemOneMaxY = itemOneMinY + firstItem.ScaleY;

            float itemTwoMinX = secondItem.Position.X;
            float itemTwoMaxX = itemTwoMinX + secondItem.ScaleX;

            float itemTwoMinY = secondItem.Position.Y;
            float itemTwoMaxY = itemTwoMinY + secondItem.ScaleY;

            return (IsBetween(itemOneMinX, itemOneMaxX, itemTwoMinX) || IsBetween(itemOneMinX, itemOneMaxX, itemTwoMaxX)) && (IsBetween(itemOneMinY, itemOneMaxY, itemTwoMinY) || IsBetween(itemOneMinY, itemOneMaxY, itemTwoMaxY));
        }

        private bool IsBetween(float minBound, float maxBound, float tested)
        {
            return tested >= minBound && tested <= maxBound;
        }

        public void ChangeContext(IScene context)
        {
            _context = context;
        }
    }
}
