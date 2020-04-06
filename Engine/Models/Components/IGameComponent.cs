using Engine.Models.GameObjects;
using Engine.Models.Scenes;

namespace Engine.Models.Components
{
    public interface IGameComponent
    {
        void Update(IGameObject entity, IScene logicContext);
    }
}
