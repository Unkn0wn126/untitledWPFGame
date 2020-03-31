#define TRACE
using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using Engine.ResourceConstants.Images;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Engine.Models.Components
{
    public class GraphicsComponent : IGraphicsComponent
    {

        public ITransformComponent Transform { get; set; }

        public ImgNames CurrentImageName { get; set; }

        public List<ImgNames> ImageNames { get; set; }

        public GraphicsComponent(List<ImgNames> imageNames, ITransformComponent transform)
        {
            ImageNames = imageNames;
            CurrentImageName = ImageNames[0];

            Transform = transform;
        }

        /// <summary>
        /// Should be called with every update of graphics
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logicContext"></param>
        public void Update(IGameObject entity, IScene logicContext)
        {
            //Position = entity.Position;
            //Width = entity.Width;
            //Height = entity.Height;
        }
    }
}
