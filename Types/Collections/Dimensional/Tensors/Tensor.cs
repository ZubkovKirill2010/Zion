using System.Collections;
using Zion.Vectors;
using Zion.Serialization;

namespace Zion
{
    public class Tensor<T> : IBinarySerializable<Tensor<T>, T>, IEnumerable<T>
    {
        private readonly T[,,] Data;

        public readonly Vector3Int Size;

        public int Width => Size.X;
        public int Height => Size.Y;
        public int Depth => Size.Z;


        public Tensor(in int Width, in int Height, in int Depth)
        {
            Data = new T[Width, Height, Depth];
            Size = new Vector3Int(Width, Height, Depth);
        }
        public Tensor(in Vector3Int Size)
        {
            Data = new T[Size.X, Size.Y, Size.Z];
            this.Size = Size;
        }


        public virtual T this[in int x, in int y, in int z]
        {
            get => Data[x, y, z];
            set => Data[x, y, z] = value;
        }
        public virtual T this[in Vector3Int Position]
        {
            get => this[Position.X, Position.Y, Position.Z];
            set => this[Position.X, Position.Y, Position.Z] = value;
        }


        public bool IsInside(in Vector3Int Position)
        {
            return Vector3Int.IsPositive(Position) && Position < Size;
        }
        public bool IsInside(in int x, in int y, in int z)
        {
            return int.IsPositive(x) && int.IsPositive(y) && int.IsPositive(z)
                && x < Size.X && y < Size.Y && z < Size.Z;
        }

        public bool IsEdge(Vector3Int Position)
        {
            return IsEdge(Position.X, Position.Y, Position.Z);
        }
        public bool IsEdge(in int x, in int y, in int z)
        {
            return x == 0 || y == 0 || z == 0
                || x == Size.X - 1 || y == Size.Y - 1 || z == Size.Z - 1;
        }

        public Tensor<T> Clone()
        {
            Tensor<T> Result = new Tensor<T>(Size);

            foreach (int x in ZEnumerable.Range(Size.X))
            {
                foreach (int y in ZEnumerable.Range(Size.Y))
                {
                    foreach (int z in ZEnumerable.Range(Size.Z))
                    {
                        Result[x, y, z] = this[x, y, z];
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
            if (IsInside(End))
            {
                throw new ArgumentOutOfRangeException($"End(={End}) is not inside of matrix(size={Size})");
            }
            if (Start == Vector3Int.Zero && End == Size)
            {
                return Clone();
            }

            Vector3Int MatrixSize = End - Start;
            Tensor<T> Result = new Tensor<T>(MatrixSize);

            foreach (int x in ZEnumerable.Range(Size.X))
            {
                foreach (int y in ZEnumerable.Range(Size.Y))
                {
                    foreach (int z in ZEnumerable.Range(Size.Z))
                    {
                        Result[x, y, z] = this[Start.X + x, Start.Z + z, Start.Y + y];
                    }
                }
            }

            return Result;
        }

        public Tensor<T> Resize(Vector3Int NewSize)
        {
            Tensor<T> Result = new Tensor<T>(NewSize);

            foreach (int x in ZEnumerable.Range(Math.Min(Width, NewSize.X)))
            {
                foreach (int y in ZEnumerable.Range(Math.Min(Height, NewSize.Y)))
                {
                    foreach (int z in ZEnumerable.Range(Math.Min(Height, NewSize.Y)))
                    {
                        Result[x, y, z] = this[x, y, z];
                    }
                }
            }

            return Result;
        }

        public void Fill(T Value)
        {
            foreach (int x in ZEnumerable.Range(Size.X))
            {
                foreach (int y in ZEnumerable.Range(Size.Y))
                {
                    foreach (int z in ZEnumerable.Range(Size.Z))
                    {
                        this[x, y, z] = Value;
                    }
                }
            }
        }
        public void Fill(Func<T> Value)
        {
            ForEach(OldValue => Value());
        }
        public void Fill(Func<int, int, int, T> Value)
        {
            foreach (int x in ZEnumerable.Range(Size.X))
            {
                foreach (int y in ZEnumerable.Range(Size.Y))
                {
                    foreach (int z in ZEnumerable.Range(Size.Z))
                    {
                        this[x, y, z] = Value(x, y, z);
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
            Tensor<I> Result = new Tensor<I>(Size);

            foreach (int x in ZEnumerable.Range(Size.X))
            {
                foreach (int y in ZEnumerable.Range(Size.Y))
                {
                    foreach (int z in ZEnumerable.Range(Size.Z))
                    {
                        Result[x, y, z] = Converter(this[x, y, z]);
                    }
                }
            }

            return Result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (int x in ZEnumerable.Range(Size.X))
            {
                foreach (int y in ZEnumerable.Range(Size.Y))
                {
                    foreach (int z in ZEnumerable.Range(Size.Z))
                    {
                        yield return this[x, y, z];
                    }
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


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
    }
}