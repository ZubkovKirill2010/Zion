using Zion.Vectors;

namespace Zion
{
    public interface IMatrix<T> : IEnumerable<T>
    {
        abstract Vector2Int Size { get; }
        abstract int Width { get; }
        abstract int Height { get; }

        abstract T this[int x, int y] { get; set; }
        abstract T this[Vector2Int Position] { get; set; }

        abstract bool IsInside(Vector2Int Position);
        abstract bool IsInside(int x, int y);
        abstract bool IsEdge(Vector2Int Position);
        abstract bool IsEdge(int x, int y);

        abstract void Fill(T Value);

        abstract void ForEach(Action<T> Action);
        abstract void ForEach(Func<T, T> Converter);
    }
}