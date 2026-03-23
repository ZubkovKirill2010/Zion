namespace Zion
{
    public interface IReader<T>
    {
        abstract bool TryRead(ObjectReader Reader, TextView View, int Start, out T Value, out int Length);
    }
}