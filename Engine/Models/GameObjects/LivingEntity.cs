using Engine.Models.Components;
using Engine.Models.Scenes;
using System.Numerics;

namespace Engine.Models.GameObjects
{
    /// <summary>
    /// Game object whose purpose is to move around
    /// the scene and to do so based on its AI or player input
    /// </summary>
    public class LivingEntity : IGameObject
    {
        private Vector2 _position;
        public double Width { get; set; }
        public double Height { get; set; }

        public IGraphicsComponent GraphicsComponent { get; set; }
        public IGameComponent PlayerMovementComponent { get; set; }
        public Vector2 Position { get => _position; set => _position = value; }

        public LivingEntity(IGraphicsComponent graphicsComponent, IGameComponent playerMovementComponent, double width, double height, Vector2 initialPosition, float baseVelocity)
        {
            GraphicsComponent = graphicsComponent;
            PlayerMovementComponent = playerMovementComponent;
            Height = width;
            Width = height;
            _position = initialPosition;
        }

        public void Update(IScene logicContext)
        {
            GraphicsComponent.Update(this, logicContext);
            PlayerMovementComponent.Update(this, logicContext);
        }
    }
}
