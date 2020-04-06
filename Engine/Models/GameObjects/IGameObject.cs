using Engine.Models.Components;
using Engine.Models.Scenes;
using System.Numerics;

namespace Engine.Models.GameObjects
{
    public interface IGameObject
    {
        public Vector2 Position { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public IGraphicsComponent GraphicsComponent { get; set; }
        public void Update(IScene logicContext);
    }
}
