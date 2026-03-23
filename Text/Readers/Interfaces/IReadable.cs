namespace Zion
{
    public interface IReadable<T> where T : IReadable<T>
    {
        public static abstract bool TryRead(ObjectReader Reader, TextView View, int Start, out T Value, out int Length);
    }
}