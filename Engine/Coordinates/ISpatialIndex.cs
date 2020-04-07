using Engine.Models.Components;
using System.Collections.Generic;

namespace Engine.Coordinates
{
    public interface ISpatialIndex
    {
        List<uint>[][] Cells { get; set; }
        void Add(uint unit, ITransformComponent position);
        List<uint> GetObjectsInRadius(ITransformComponent focus, int cellRadius);
        void Move(uint unit, ITransformComponent position, float x, float y);
    }
}
