using System.Collections;
using Zion.Vectors;

namespace Zion
{
    public class Matrix<T> : IMatrix<T>, IBinaryGeneric<Matrix<T>, T>, IEnumerable<T>
    {
        private readonly T[,] Data;
        public Vector2Int Size { get; }
        public int Width => Size.x;
        public int Height => Size.y;


        public Matrix(Vector2Int Size)
        {
            Data = new T[Size.x, Size.y];
            this.Size = Size;
        }
        public Matrix(int Width, int Height)
        {
            Data = new T[Width, Height];
            Size = new Vector2Int(Width, Height);
        }
        public Matrix(int SizeLength)
        {
            Data = new T[SizeLength, SizeLength];
            Size = new Vector2Int(SizeLength);
        }
        private Matrix(Vector2Int Size, Func<T> GetValue)
        {
            this.Size = Size;
            Data = new T[Size.x, Size.y];
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Data[x, y] = GetValue();
                }
            }
        }

        public virtual T this[int x, int y]
        {
            get => Data[x, y];
            set => Data[x, y] = value;
        }
        public T this[Vector2Int Position]
        {
            get => this[Position.x, Position.y];
            set => this[Position.x, Position.y] = value;
        }

        public bool IsInside(Vector2Int Position)
        {
            return Position >= Vector2Int.Zero && Position < Size;
        }
        public bool IsInside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Size.x && y < Size.y;
        }

        public bool IsEdge(Vector2Int Position)
        {
            return IsEdge(Position.x, Position.y);
        }
        public bool IsEdge(int x, int y)
        {
            return x == 0 || x == Size.x - 1 && y == 0 || y == Size.y - 1;
        }

        public Matrix<T> Clone()
        {
            Matrix<T> Result = new Matrix<T>(Size);

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Result[x, y] = this[x, y];
                }
            }

            return Result;
        }
        public Matrix<T> GetSubMatrix(Vector2Int Start, Vector2Int End)
        {
            if (!IsInside(Start))
            {
                throw new ArgumentOutOfRangeException($"Start(={Start}) is not inside of matrix(size={Size})");
            }
            if (IsInside(End))
            {
                throw new ArgumentOutOfRangeException($"End(={End}) is not inside of matrix(size={Size})");
            }
            if (Start == Vector2Int.Zero && End == Size)
            {
                return Clone();
            }

            Vector2Int MatrixSize = End - Start;
            Matrix<T> Result = new Matrix<T>(MatrixSize);

            for (int x = 0; x < MatrixSize.x; x++)
            {
                for (int y = 0; y < MatrixSize.y; y++)
                {
                    Result[x, y] = this[Start.x + x, Start.y + y];
                }
            }

            return Result;
        }

        public void Fill(T Value)
        {
            ForEach(OldValue => Value);
        }
        public void FillChessPattern(T A, T B)
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    this[x, y] = (x + y).IsEven() ? A : B;
                }
            }
        }

        public void SetEdge(T Value)
        {
            int Down = Size.y - 1;
            int Right = Size.x - 1;

            for (int i = 0; i < Size.x; i++)
            {
                Data[i, 0] = Value;
                Data[i, Down] = Value;
            }
            for (int i = 1; i < Size.y; i++)
            {
                Data[0, i] = Value;
                Data[Right, i] = Value;
            }
        }

        public void ForEach(Action<T> Action)
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Action(this[x, y]);
                }
            }
        }
        public void ForEach(Func<T, T> Converter)
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    this[x, y] = Converter(this[x, y]);
                }
            }
        }

        public Matrix<I> Convert<I>(Converter<T, I> Converter)
        {
            Matrix<I> Result = new Matrix<I>(Size);

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Result[x, y] = Converter(this[x, y]);
                }
            }

            return Result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    yield return this[x, y];
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public void Write(BinaryWriter Writer, Action<BinaryWriter, T> Write)
        {
            Writer.Write(Size);
            foreach (T Item in this)
            {
                Write(Writer, Item);
            }
        }
        public static Matrix<T> Read(BinaryReader Reader, Func<BinaryReader, T> Read)
        {
            return new Matrix<T>
            (
                Reader.Read<Vector2Int>(),
                () => Read(Reader)
            );
        }
    }
}