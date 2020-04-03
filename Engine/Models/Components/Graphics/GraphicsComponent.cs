using Engine.ResourceConstants.Images;

namespace Engine.Models.Components
{
    public class GraphicsComponent : IGraphicsComponent
    {
        public ImgNames CurrentImageName { get; set; }

        public GraphicsComponent(ImgNames imgName)
        {
            CurrentImageName = imgName;
        }

    }
}
