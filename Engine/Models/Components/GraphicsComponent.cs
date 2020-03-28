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
        private ImgNames _currentImageName;
        private List<ImgNames> _imageNames;
        private Vector2 _position;
        private double _width;
        private double _height;

        public ImgNames CurrentImageName
        {
            get { return _currentImageName; }
            set { _currentImageName = value; }
        }

        public List<ImgNames> ImageNames { get => _imageNames; set => _imageNames = value; }
        public Vector2 Position { get => _position; set => _position = value; }
        public double Width { get => _width; set => _width = value; }
        public double Height { get => _height; set => _height = value; }

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
