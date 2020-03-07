using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace Adapter.Graphics
{
    public interface IGraphicsAdapter
    {
        public double Height { get; }
        public double Width { get; }

        public void Draw(IGraphicsComponentAdapter<Rectangle> data);

        public void Clear();
    }
}
