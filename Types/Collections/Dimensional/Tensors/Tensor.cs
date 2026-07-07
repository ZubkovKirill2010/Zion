using System.Collections;
using Zion.Serialization;
using Zion.Vectors;

namespace Zion
{
    public class Tensor<T> : IBinarySerializable<Tensor<T>, T>, IEnumerable<T>
    {
        #region Data
        private readonly T[,,] Data;
        public  readonly Vector3Int Size;

        #endregion

        #region Properties
        public int Width  => Size.X;
        public int Height => Size.Y;
        public int Depth  => Size.Z;

        #endregion

        #region Constructors
        public Tensor(int Width, int Height, int Depth)
        {
            Data = new T[Width, Height, Depth];
            Size = new Vector3Int(Width, Height, Depth);
        }

        public Tensor(Vector3Int Size)
        {
            this.Data = new T[Size.X, Size.Y, Size.Z];
            this.Size = Size;
        }

        #endregion

        #region Indexers
        public virtual T this[in int X, in int Y, in int Z]
        {
            get => Data[X, Y, Z];
            set => Data[X, Y, Z] = value;
        }

        public virtual T this[in Vector3Int Position]
        {
            get => this[Position.X, Position.Y, Position.Z];
            set => this[Position.X, Position.Y, Position.Z] = value;
        }

        #endregion

        #region PublicMethods
        public bool IsInside(in Vector3Int Position)
        {
            return Vector3Int.IsPositive(Position) && Position < Size;
        }

        public bool IsInside(in int X, in int Y, in int Z)
        {
            return int.IsPositive(X) && int.IsPositive(Y) && int.IsPositive(Z)
                && X < Size.X && Y < Size.Y && Z < Size.Z;
        }


        public bool IsEdge(in Vector3Int Position)
        {
            return IsEdge(Position.X, Position.Y, Position.Z);
        }

        public bool IsEdge(in int X, in int Y, in int Z)
        {
            return X == 0 || Y == 0 || Z == 0
                || X == Size.X - 1 || Y == Size.Y - 1 || Z == Size.Z - 1;
        }


        public Tensor<T> Clone()
        {
            Tensor<T> Result = new Tensor<T>(Size);

            foreach (int X in ZEnumerable.Range(Size.X))
            {
                foreach (int Y in ZEnumerable.Range(Size.Y))
                {
                    foreach (int Z in ZEnumerable.Range(Size.Z))
                    {
                        Result[X, Y, Z] = this[X, Y, Z];
                    }
                }
            }

            return Result;
        }

        public Tensor<T> GetSubMatrix(Vector3Int Start, Vector3Int End)
        {
            if (!IsInside(Start))
            {
                throw new ArgumentOutOfRangeException($"Start(={Start}) is not inside of matrix(size={Size})");
            }
            if (!IsInside(End))
            {
                throw new ArgumentOutOfRangeException($"End(={End}) is not inside of matrix(size={Size})");
            }
            if (Start == Vector3Int.Zero && End == Size)
            {
                return Clone();
            }

            Vector3Int MatrixSize = End - Start;
            Tensor<T> Result = new Tensor<T>(MatrixSize);

            for (int x = 0; x < MatrixSize.X; x++)
            {
                for (int y = 0; y < MatrixSize.Y; y++)
                {
                    for (int z = 0; z < MatrixSize.Z; z++)
                    {
                        Result[x, y, z] = this[Start.X + x, Start.Y + y, Start.Z + z];
                    }
                }
            }

            return Result;
        }

        public Tensor<T> Resize(Vector3Int NewSize)
        {
            Tensor<T> Result = new Tensor<T>(NewSize);

            foreach (int X in ZEnumerable.Range(Math.Min(Width, NewSize.X)))
            {
                foreach (int Y in ZEnumerable.Range(Math.Min(Height, NewSize.Y)))
                {
                    foreach (int Z in ZEnumerable.Range(Math.Min(Depth, NewSize.Z)))
                    {
                        Result[X, Y, Z] = this[X, Y, Z];
                    }
                }
            }

            return Result;
        }


        public void Fill(T Value)
        {
            Vector3Int Size = this.Size;
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int z = 0; z < Size.Z; z++)
                    {
                        Data[x, y, z] = Value;
                    }
                }
            }
        }

        public void Fill(Func<T> Value)
        {
            Vector3Int Size = this.Size;
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int z = 0; z < Size.Z; z++)
                    {
                        Data[x, y, z] = Value();
                    }
                }
            }
        }

        public void Fill(Func<int, int, int, T> Function)
        {
            Vector3Int Size = this.Size;
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int z = 0; z < Size.Z; z++)
                    {
                        Data[x, y, z] = Function(x, y, z);
                    }
                }
            }
        }

        public void Fill(Func<T, T> Function)
        {
            Vector3Int Size = this.Size;
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int z = 0; z < Size.Z; z++)
                    {
                        Data[x, y, z] = Function(Data[x, y, z]);
                    }
                }
            }
        }


        public void ForEach(Action<T> Action)
        {
            foreach (T Item in this)
            {
                Action(Item);
            }
        }


        public Tensor<I> Convert<I>(Converter<T, I> Converter)
        {
            Vector3Int Size = this.Size;
            Tensor<I> Result = new Tensor<I>(Size);

            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int z = 0; z < Size.Z; z++)
                    {
                        Result[x, y, z] = Converter(Data[x, y, z]);
                    }
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

        public static Tensor<T> Read(BinaryReader Reader, Func<T> Read)
        {
            Tensor<T> Result = new Tensor<T>(Reader.Read<Vector3Int>());

            Result.Fill(Read);

            return Result;
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            Vector3Int Size = this.Size;

            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int z = 0; z < Size.Z; z++)
                    {
                        yield return Data[x, y, z];
                    }
                }
            }
        }

        #endregion
    }
}