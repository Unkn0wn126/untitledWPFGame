#define TRACE
using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using System.Collections.Generic;
using System.Numerics;

namespace Engine.Models.Components
{
    public class GraphicsComponent : IGraphicsComponent
    {
        public string CurrentImageName { get; set; }
        public List<string> ImageNames { get; set; }
        public Vector2 Position { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public GraphicsComponent(List<string> imageNames)
        {
            ImageNames = imageNames;
            CurrentImageName = ImageNames[0];
        }

        /// <summary>
        /// Should be called with every update of graphics
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logicContext"></param>
        public void Update(IGameObject entity, IScene logicContext)
        {
            Position = entity.Position;
            Width = entity.Width;
            Height = entity.Height;
        }
    }
}
