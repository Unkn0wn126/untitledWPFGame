using Engine.ResourceConstants.Images;

namespace Engine.Models.Components
{
    public interface IGraphicsComponent : IGameComponent
    {
        public ImgNames CurrentImageName { get; set; }
    }
}
