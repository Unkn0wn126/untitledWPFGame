using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.MovementStateStrategies
{
    public interface IMovementStrategy
    {
        public void ExecuteStrategy(IGameObject entity);
    }
}
