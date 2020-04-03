using Engine.Models.Components;
using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Coordinates
{
    public interface ISpatialIndex
    {
        public List<IGameObject>[][] Cells { get; set; }
        public void Add(IGameObject unit);
        public List<IGameObject> GetObjectsInRadius(ITransformComponent focus, int cellRadius);
        public List<IGraphicsComponent> GetGraphicsComponentsInRadius(ITransformComponent focus, int cellRadius);
        public void Move(IGameObject unit, float x, float y);
    }
}
