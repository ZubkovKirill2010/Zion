namespace Zion
{
    public interface IReadable<T> where T : IReadable<T>
    {
        public static abstract bool TryRead(ObjectReader Reader, int Start, out T Value, out int Length);
    }
}