using System.Collections;
using Zion.Serialization;
using Zion.Vectors;

namespace Zion
{
    public class Matrix<T> : IMatrix<T>, IBinarySerializable<Matrix<T>, T>
    {
        #region Data
        private readonly T[,] Data;

        public Vector2Int Size { get; }

        #endregion

        #region Properties
        public int Width => Size.X;
        public int Height => Size.Y;

        #endregion

        #region Constructors
        public Matrix(int Side)
        {
            Data = new T[Side, Side];
            Size = new Vector2Int(Side);
        }
        public Matrix(int Side, T Value) : this(Side)
        {
            Fill(Value);
        }
        public Matrix(int Side, Func<T> GetValue) : this(Side)
        {
            Fill(GetValue);
        }

        public Matrix(int Width, int Height)
        {
            Data = new T[Width, Height];
            Size = new Vector2Int(Width, Height);
        }
        public Matrix(int Width, int Height, T Value) : this(Width, Height)
        {
            Fill(Value);
        }
        public Matrix(int Width, int Height, Func<T> GetValue) : this(Width, Height)
        {
            Fill(GetValue);
        }

        public Matrix(Vector2Int Size)
        {
            Data = new T[Size.X, Size.Y];
            this.Size = Size;
        }
        public Matrix(Vector2Int Size, T Value) : this(Size)
        {
            Fill(Value);
        }
        public Matrix(Vector2Int Size, Func<T> GetValue) : this(Size)
        {
            Fill(GetValue);
        }

        #endregion

        #region Indexers
        public virtual T this[int X, int Y]
        {
            get => Data[X, Y];
            set => Data[X, Y] = value;
        }
        public T this[Vector2Int Position]
        {
            get => this[Position.X, Position.Y];
            set => this[Position.X, Position.Y] = value;
        }

        #endregion

        #region IMatrix
        public bool IsInside(Vector2Int Position)
        {
            return Position >= Vector2Int.Zero && Position < Size;
        }

        public bool IsInside(int X, int Y)
        {
            return X >= 0 && Y >= 0 && X < Size.X && Y < Size.Y;
        }


        public bool IsEdge(Vector2Int Position)
        {
            return IsEdge(Position.X, Position.Y);
        }

        public bool IsEdge(int X, int Y)
        {
            return X == 0 || X == Width - 1 || Y == 0 || Y == Height - 1;
        }


        public Matrix<T> Clone()
        {
            Matrix<T> Result = new Matrix<T>(Size);

            for (int X = 0; X < Size.X; X++)
            {
                for (int Y = 0; Y < Size.Y; Y++)
                {
                    Result[X, Y] = this[X, Y];
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
            if (!IsInside(End))
            {
                throw new ArgumentOutOfRangeException($"End(={End}) is not inside of matrix(size={Size})");
            }
            if (Start == Vector2Int.Zero && End == Size)
            {
                return Clone();
            }

            Vector2Int MatrixSize = End - Start;
            Matrix<T> Result = new Matrix<T>(MatrixSize);

            for (int X = 0; X < MatrixSize.X; X++)
            {
                for (int Y = 0; Y < MatrixSize.Y; Y++)
                {
                    Result[X, Y] = this[Start.X + X, Start.Y + Y];
                }
            }

            return Result;
        }

        public Matrix<T> Resize(Vector2Int NewSize)
        {
            Matrix<T> Result = new Matrix<T>(NewSize);

            foreach (int X in ZEnumerable.Range(0, Math.Min(Width, NewSize.X)))
            {
                foreach (int Y in ZEnumerable.Range(0, Math.Min(Height, NewSize.Y)))
                {
                    Result[X, Y] = this[X, Y];
                }
            }

            return Result;
        }


        public void Fill(T Value)
        {
            for (int X = 0; X < Size.X; X++)
            {
                for (int Y = 0; Y < Size.Y; Y++)
                {
                    this[X, Y] = Value;
                }
            }
        }

        public void Fill(Func<T> Value)
        {
            for (int X = 0; X < Size.X; X++)
            {
                for (int Y = 0; Y < Size.Y; Y++)
                {
                    this[X, Y] = Value();
                }
            }
        }

        public void Fill(Func<int, int, T> Value)
        {
            for (int X = 0; X < Size.X; X++)
            {
                for (int Y = 0; Y < Size.Y; Y++)
                {
                    this[X, Y] = Value(X, Y);
                }
            }
        }

        public void FillChessPattern(T A, T B)
        {
            for (int X = 0; X < Size.X; X++)
            {
                for (int Y = 0; Y < Size.Y; Y++)
                {
                    this[X, Y] = (X + Y).IsEven() ? A : B;
                }
            }
        }


        public void SetEdge(T Value)
        {
            int Down = Size.Y - 1;
            int Right = Size.X - 1;

            for (int i = 0; i < Size.X; i++)
            {
                Data[i, 0] = Value;
                Data[i, Down] = Value;
            }
            for (int i = 1; i < Size.Y; i++)
            {
                Data[0, i] = Value;
                Data[Right, i] = Value;
            }
        }

        public void ForEach(Action<T> Action)
        {
            for (int X = 0; X < Size.X; X++)
            {
                for (int Y = 0; Y < Size.Y; Y++)
                {
                    Action(this[X, Y]);
                }
            }
        }

        public Matrix<I> Convert<I>(Converter<T, I> Converter)
        {
            Matrix<I> Result = new Matrix<I>(Size);

            for (int X = 0; X < Size.X; X++)
            {
                for (int Y = 0; Y < Size.Y; Y++)
                {
                    Result[X, Y] = Converter(this[X, Y]);
                }
            }

            return Result;
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer, Action<T> Write)
        {
            Writer.Write(Size);
            foreach (T Item in this)
            {
                Write(Item);
            }
        }

        public static Matrix<T> Read(BinaryReader Reader, Func<T> Read)
        {
            return new Matrix<T>
            (
                Reader.Read<Vector2Int>(),
                () => Read()
            );
        }

        #endregion

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            for (int X = 0; X < Width; X++)
            {
                for (int Y = 0; Y < Height; Y++)
                {
                    yield return this[X, Y];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}