namespace Zion
{
    public interface IReader<T>
    {
        abstract bool TryRead(ObjectReader Reader, int Start, out T Value, out int Length);
    }
}