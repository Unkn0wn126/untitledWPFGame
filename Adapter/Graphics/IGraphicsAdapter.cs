using System.Windows.Shapes;

namespace Adapter.Graphics
{
    public interface IGraphicsAdapter
    {
        double Height { get; }
        double Width { get; }
        void Draw(IGraphicsComponentAdapter<Rectangle> data);
        void Clear();
    }
}
