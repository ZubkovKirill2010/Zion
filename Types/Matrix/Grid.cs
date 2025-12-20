using Zion.Vectors;

namespace Zion
{
    public sealed class Grid<T>
    {
        private const int ChunkSize = 16;
        private const int ChunkFilter = 15;
        private const int ChunkShift = 4;

        private readonly Dictionary<Vector2Int, Matrix<T>> Chunks;
        public T Base { private get; init; } = default;


        public Grid()
        {
            Chunks = new Dictionary<Vector2Int, Matrix<T>>(80);
        }
        public Grid(int Capacity)
        {
            Chunks = new Dictionary<Vector2Int, Matrix<T>>(Capacity);
        }


        public T this[Vector2Int Position]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIf(!IsInside(Position), $"Position {Position} out of range [ [0, 0] - [{int.MaxValue}, {int.MaxValue}] ]");

                if (Chunks.TryGetValue(Position >> ChunkShift, out Matrix<T>? Chunk))
                {
                    return Chunk[Position.x & ChunkFilter, Position.y & ChunkFilter];
                }
                return Base;
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIf(!IsInside(Position), $"Position {Position} out of range [ [0, 0] - [{int.MaxValue}, {int.MaxValue}] ]");

                Vector2Int ChunkPosition = Position >> ChunkShift;

                Matrix<T> Chunk = Chunks.TryGetValue(ChunkPosition, out Matrix<T>? Matrix)
                    ? Matrix
                    : Accessor.AddAndReturn(Chunks, ChunkPosition, new Matrix<T>(ChunkSize));

                Chunk[Position.x & ChunkFilter, Position.y & ChunkFilter] = value;
            }
        }
        public T this[int x, int y]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIf(!IsInside(x, y), $"Position [{x}, {y}] out of range [ [0, 0] - [{int.MaxValue}, {int.MaxValue}] ]");

                if (Chunks.TryGetValue(new Vector2Int(x >> ChunkShift, y >> ChunkShift), out Matrix<T>? Chunk))
                {
                    return Chunk[x & ChunkFilter, y & ChunkFilter];
                }
                return Base;
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIf(!IsInside(x, y), $"Position [{x}, {y}] out of range [ [0, 0] - [{int.MaxValue}, {int.MaxValue}] ]");

                Vector2Int ChunkPosition = new Vector2Int(x >> ChunkShift, y >> ChunkShift);

                Matrix<T> Chunk = Chunks.TryGetValue(ChunkPosition, out Matrix<T>? Matrix)
                    ? Matrix
                    : Accessor.AddAndReturn(Chunks, ChunkPosition, new Matrix<T>(ChunkSize));

                Chunk[x & ChunkFilter, y & ChunkFilter] = value;
            }
        }


        public void Clear()
        {
            Chunks.Clear();
        }


        public bool IsInside(Vector2Int Position)
        {
            return IsInside(Position.x, Position.y);
        }

        public bool IsInside(int x, int y)
        {
            return int.IsPositive(x) && int.IsPositive(y);
        }
    }
}