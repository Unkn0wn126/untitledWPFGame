using System.Windows.Media;
using System.Windows.Shapes;

namespace Adapter.Graphics
{
    public class GraphicsComponentAdapter : IGraphicsComponentAdapter<Rectangle>
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public Rectangle Data { get; set; }
        public double XPos { get; set; }
        public double YPos { get; set; }

        private IGraphicsAdapter _context;

        public GraphicsComponentAdapter(IGraphicsAdapter context)
        {
            _context = context;
            Data = new Rectangle();
            Data.Fill = new SolidColorBrush(Colors.AliceBlue);
        }

        public void Draw()
        {
            _context.Draw(this);
        }
    }
}
