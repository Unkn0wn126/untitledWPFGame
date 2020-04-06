namespace Adapter.Graphics
{
    public interface IGraphicsComponentAdapter<T>
    {
        public T Data { get; set; }
        public double XPos { get; set; }
        public double YPos { get; set; }
    }
}
