using Engine.Models.Components;
using System.Collections.Generic;

namespace Engine.Coordinates
{
    public interface ISpatialIndex
    {
        public List<uint>[][] Cells { get; set; }
        public void Add(uint unit, ITransformComponent position);
        public List<uint> GetObjectsInRadius(ITransformComponent focus, int cellRadius);
        public void Move(uint unit, ITransformComponent position, float x, float y);
    }
}
