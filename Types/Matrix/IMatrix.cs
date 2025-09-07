using Zion.Vectors;

namespace Zion
{
    public interface IMatrix<T> : IEnumerable<T>
    {
        public abstract Vector2Int Size { get; }
        public abstract int Width { get; }
        public abstract int Height { get; }

        public abstract T this[int x, int y] { get; set; }
        public abstract T this[Vector2Int Position] { get; set; }

        public abstract bool IsInside(Vector2Int Position);
        public abstract bool IsInside(int x, int y);
        public abstract bool IsEdge(Vector2Int Position);
        public abstract bool IsEdge(int x, int y);

        public abstract void Fill(T Value);

        public abstract void ForEach(Action<T> Action);
        public abstract void ForEach(Func<T, T> Converter);
    }
}