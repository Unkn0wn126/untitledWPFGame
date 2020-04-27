using Engine.Models.Components;
using System.Collections.Generic;
using System.Numerics;

namespace Engine.Coordinates
{
    public interface ISpatialIndex
    {
        List<uint>[][] Cells { get; set; }
        void Add(uint unit, Vector2 position);
        List<uint> GetObjectsInRadius(ITransformComponent focus, int cellRadius);
        void Move(uint unit, Vector2 oldPos, Vector2 newPos);
        void Remove(uint unit, ITransformComponent entityTransform);
    }
}
