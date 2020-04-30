using ResourceManagers.Images;

namespace Engine.Models.Components
{
    /// <summary>
    /// Component used for rendering
    /// the entity in the graphics engine
    /// </summary>
    public interface IGraphicsComponent : IGameComponent
    {
        ImgName CurrentImageName { get; set; }
    }
}
