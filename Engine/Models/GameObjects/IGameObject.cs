using Engine.Models.Components;
using Engine.Models.Scenes;
using System.Numerics;

namespace Engine.Models.GameObjects
{
    public interface IGameObject
    {
        Vector2 Position { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        IGraphicsComponent GraphicsComponent { get; set; }
        void Update(IScene logicContext);
    }
}
