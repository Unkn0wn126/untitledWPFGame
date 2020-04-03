using Engine.Models.GameObjects;
using Engine.Models.MovementStateStrategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components
{
    public interface IEntityMovementComponent : IGameComponent
    {
        public void SetMovementState(IMovementStrategy newMovementStrategy);
        public void Update(ILivingEntity entity);
    }
}
