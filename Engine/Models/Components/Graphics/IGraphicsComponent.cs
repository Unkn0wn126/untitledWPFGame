using Engine.ResourceConstants.Images;

namespace Engine.Models.Components
{
    public interface IGraphicsComponent : IGameComponent
    {
        ImgNames CurrentImageName { get; set; }
    }
}
