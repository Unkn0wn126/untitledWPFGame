using Engine.Coordinates;
using Engine.Models.Components;
using System.Numerics;

namespace Engine.Models.MovementStateStrategies
{
    public class MovementUpRight : IMovementStrategy
    {
        private float _xModifier;
        private float _yModifier;

        public MovementUpRight()
        {
            _xModifier = 1;
            _yModifier = -1;
        }
        public void ExecuteStrategy(uint entity, ITransformComponent transform, ISpatialIndex grid)
        {
            float baseVelocity = 5f;
            Vector2 newPos = transform.Position;
            newPos.X += baseVelocity * _xModifier;
            newPos.Y += baseVelocity * _yModifier;

            grid.Move(entity, transform, newPos.X, newPos.Y);
        }
    }
}
