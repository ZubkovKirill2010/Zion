namespace Zion
{
    public interface IRange<T>
    {
        public T Start { get; }
        public T End { get; }

        public bool IsInside(T Value);

        public bool IsInside<R>(R Range) where R : IRange<T>;

        public bool Overlap<R>(R Range) where R : IRange<T>;
    }
}