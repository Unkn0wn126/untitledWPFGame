using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using Engine.ResourceConstants.Images;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Models.Components
{
    public interface IGraphicsComponent : IGameComponent
    {
        public Vector2 Position { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public ImgNames CurrentImageName { get; set; }
        public List<ImgNames> ImageNames { get; set; }
    }
}
