namespace Zion
{
    public interface IReadable<T> where T : IReadable<T>
    {
        abstract static bool TryRead(ObjectReader Reader, int Start, out T Value, out int Length);
    }
}