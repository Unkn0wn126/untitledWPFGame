using System.Windows.Controls;
using System.Windows.Shapes;

namespace Adapter.Graphics
{
    public class GraphicsAdapter : IGraphicsAdapter
    {
        private Canvas _context;
        public double Height { get { return _context.ActualHeight; } }
        public double Width { get { return _context.ActualWidth; } }
        public GraphicsAdapter(Canvas context)
        {
            _context = context;
        }

        public void Draw(IGraphicsComponentAdapter<Rectangle> data)
        {
            Canvas.GetLeft(data.Data);
            Canvas.SetLeft(data.Data, data.XPos);

            Canvas.GetTop(data.Data);
            Canvas.SetTop(data.Data, data.YPos);
            _context.Children.Add(data.Data);

        }

        public void Clear()
        {
            _context.Children.Clear();
        }
    }
}
