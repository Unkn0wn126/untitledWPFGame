using Engine.Models.Components;
using Engine.Models.Scenes;
using System.Numerics;

namespace Engine.Models.GameObjects
{
    /// <summary>
    /// Object for representing a walkable ground
    /// </summary>
    public class Ground : IGameObject
    {
        private Vector2 _position;
        public double Width { get; set; }
        public double Height { get; set; }

        public IGraphicsComponent GraphicsComponent { get; set; }
        public Vector2 Position { get => _position; set => _position = value; }

        public Ground(IGraphicsComponent graphicsComponent, Vector2 position, double width, double height)
        {
            GraphicsComponent = graphicsComponent;
            Position = position;
            Width = width;
            Height = height;
        }

        public void Update(IScene logicContext)
        {
            GraphicsComponent.Update(this, logicContext);
        }
    }
}
