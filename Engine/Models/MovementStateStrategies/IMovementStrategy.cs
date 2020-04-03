using Engine.Coordinates;
using Engine.Models.Components;

namespace Engine.Models.MovementStateStrategies
{
    public interface IMovementStrategy
    {
        public void ExecuteStrategy(uint entity, ITransformComponent transform, ISpatialIndex grid);
    }
}
