using Engine.ResourceConstants.Images;

namespace Engine.Models.Components
{
    public class GraphicsComponent : IGraphicsComponent
    {
        public ImgName CurrentImageName { get; set; }

        public GraphicsComponent(ImgName imgName)
        {
            CurrentImageName = imgName;
        }

    }
}
