using Zion.Vectors;

namespace Zion
{
    public interface IMatrix<T> : IEnumerable<T>
    {
        public Vector2Int Size { get; }
        public int Width { get; }
        public int Height { get; }

        public T this[int X, int Y] { get; set; }
        public T this[Vector2Int Position] { get; set; }

        public bool IsInside(Vector2Int Position)
        {
            return IsInside(Position.X, Position.Y);
        }

        public bool IsInside(int X, int Y)
        {
            return X >= 0 && X < Width && Y >= 0 && Y < Height;
        }


        public bool IsEdge(Vector2Int Position)
        {
            return IsEdge(Position.X, Position.Y);
        }

        public bool IsEdge(int X, int Y)
        {
            return X == 0 || X == Width - 1 || Y == 0 || Y == Height - 1;
        }

        public void Fill(T Value);

        public void ForEach(Action<T> Action);
    }
}