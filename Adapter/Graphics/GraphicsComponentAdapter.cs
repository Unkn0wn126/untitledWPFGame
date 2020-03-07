using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Adapter.Graphics
{
    public class GraphicsComponentAdapter : IGraphicsComponentAdapter<Rectangle>
    {
        private Rectangle _data;
        private double _xPos;
        private double _yPos;
        private double width;
        private double height;

        public double Height
        {
            get { return height; }
            set { height = value; }
        }


        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public Rectangle Data { get => _data; set => _data = value; }
        public double XPos { get => _xPos; set => _xPos = value; }
        public double YPos { get => _yPos; set => _yPos = value; }

        private IGraphicsAdapter _context;

        public GraphicsComponentAdapter(IGraphicsAdapter context)
        {
            _context = context;
            _data = new Rectangle();
            _data.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
        }

        public void Draw()
        {
            _context.Draw(this);
        }
    }
}
