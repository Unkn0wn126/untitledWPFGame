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
        private Vector2 _position;

        public ImgNames CurrentImageName { get; set; }

        public List<ImgNames> ImageNames { get; set; }
        public Vector2 Position { get => _position; set => _position = value; }
        public float Width { get; set; }
        public float Height { get; set; }

        public GraphicsComponent(List<ImgNames> imageNames)
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
