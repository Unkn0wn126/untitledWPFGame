namespace Adapter.Graphics
{
    public interface IGraphicsComponentAdapter<T>
    {
       T Data { get; set; }
       double XPos { get; set; }
       double YPos { get; set; }
    }
}
