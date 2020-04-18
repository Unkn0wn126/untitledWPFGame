using ResourceManagers.Images;

namespace Engine.Models.Components
{
    public interface IGraphicsComponent : IGameComponent
    {
        ImgName CurrentImageName { get; set; }
    }
}
