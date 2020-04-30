using Engine.Models.Components;
using System.Collections.Generic;
using System.Numerics;

namespace Engine.Coordinates
{
    /// <summary>
    /// Used for spatial indexing.
    /// Speeds up the process of
    /// filtering and processing entities
    /// by grouping them into smaller groups
    /// based on their position
    /// </summary>
    public interface ISpatialIndex
    {
        List<uint>[][] Cells { get; set; }

        /// <summary>
        /// Adds the entity to the spatial index
        /// based on its current position
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="position"></param>
        void Add(uint unit, Vector2 position);

        /// <summary>
        /// Gets all of the entities in a given
        /// radius from the provided position
        /// </summary>
        /// <param name="focus"></param>
        /// <param name="cellRadius"></param>
        /// <returns></returns>
        List<uint> GetObjectsInRadius(ITransformComponent focus, int cellRadius);

        /// <summary>
        /// Moves the given entity from
        /// its old position to a new one
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="oldPos"></param>
        /// <param name="newPos"></param>
        void Move(uint unit, Vector2 oldPos, Vector2 newPos);

        /// <summary>
        /// Removes the given entity
        /// from this spatial index
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="entityTransform"></param>
        void Remove(uint unit, ITransformComponent entityTransform);
    }
}
