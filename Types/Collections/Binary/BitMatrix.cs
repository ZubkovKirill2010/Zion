using System.Collections;
using Zion.Serialization;
using Zion.Vectors;

namespace Zion
{
    public sealed class BitMatrix : IMatrix<bool>, IBinarySerializable<BitMatrix>
    {
        #region Constants
        public const int Filter = 0b111;

        #endregion

        #region Data
        private readonly byte[] Data;
        private readonly int Length;

        public Vector2Int Size { get; }

        #endregion

        #region Properties
        public int Width => Size.X;
        public int Height => Size.Y;

        #endregion

        #region Constructors
        public BitMatrix(Vector2Int Size)
        {
            ArgumentOutOfRangeException.ThrowIf(Vector2Int.IsNegative(Size), nameof(Size), $"Size(={Size}) can not be negative");
            
            this.Size = Size;
            this.Length = Size.X * Size.Y;
            this.Data = new byte[GetByteCount(Length)];
        }

        public BitMatrix(int Width, int Height)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Width);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Height);

            this.Size = new Vector2Int(Width, Height);
            this.Length = Width * Height;
            this.Data = new byte[GetByteCount(Length)];
        }

        public BitMatrix(int Side)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Side);

            this.Size = new Vector2Int(Side);
            this.Length = Side * Side;
            this.Data = new byte[GetByteCount(Length)];
        }

        private BitMatrix(byte[] Data, Vector2Int Size)
        {
            this.Data = Data;
            this.Length = Size.X * Size.Y;
            this.Size = Size;
        }

        #endregion

        #region Indexers
        public bool this[int X, int Y]
        {
            get
            {
                GetBitPosition(X, Y, out int Byte, out int Bit);
                return Data[Byte].GetBit(Bit);
            }
            set
            {
                GetBitPosition(X, Y, out int Byte, out int Bit);
                Data[Byte] = Data[Byte].SetBit(Bit, value);
            }
        }

        public bool this[Vector2Int Position]
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


        public BitMatrix Clone()
        {
            return new BitMatrix(ZArray.Clone(Data), Size);
        }

        public BitMatrix GetSubMatrix(Vector2Int Start, Vector2Int End)
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
            BitMatrix Result = new BitMatrix(MatrixSize);

            for (int X = 0; X < MatrixSize.X; X++)
            {
                for (int Y = 0; Y < MatrixSize.Y; Y++)
                {
                    Result[X, Y] = this[Start.X + X, Start.Y + Y];
                }
            }

            return Result;
        }


        public void Fill(bool Value)
        {
            byte Byte = Value ? byte.MaxValue : byte.MinValue;

            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = Byte;
            }
        }


        public void ForEach(Action<bool> Action)
        {
            for (int X = 0; X < Size.X; X++)
            {
                for (int Y = 0; Y < Size.Y; Y++)
                {
                    Action(this[X, Y]);
                }
            }
        }

        public Matrix<I> Convert<I>(Converter<bool, I> Converter)
        {
            Vector2Int Size = this.Size;
            Matrix<I> Result = new Matrix<I>(Size);

            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    Result[x, y] = Converter(this[x, y]);
                }
            }

            return Result;
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<bool> GetEnumerator()
        {
            int FullBytes = Length >> 3;

            for (int i = 0; i < FullBytes; i++)
            {
                byte Current = Data[i];
                yield return (Current & 0b0000_0001) != 0;
                yield return (Current & 0b0000_0010) != 0;
                yield return (Current & 0b0000_0100) != 0;
                yield return (Current & 0b0000_1000) != 0;
                yield return (Current & 0b0001_0000) != 0;
                yield return (Current & 0b0010_0000) != 0;
                yield return (Current & 0b0100_0000) != 0;
                yield return (Current & 0b1000_0000) != 0;
            }

            int LastByteLength = Length & Filter;
            if (LastByteLength != 0)
            {
                byte Current = Data[FullBytes];
                int BitIndex = 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0000_0001) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0000_0010) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0000_0100) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0000_1000) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0001_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0010_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b0100_0000) != 0;
                if (++BitIndex > LastByteLength) { yield break; } yield return (Current & 0b1000_0000) != 0;
            }
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Size);
            Writer.WriteCollection(Data);
        }

        public static BitMatrix Read(BinaryReader Reader)
        {
            Vector2Int Size = Reader.Read<Vector2Int>();
            byte[] Data = Reader.ReadArray<byte>();

            return new BitMatrix(Data, Size);
        }

        #endregion

        #region PrivateMethods
        private static int GetByteCount(int Count)
        {
            return (Count + 7) >> 3;
        }

        private void GetBitPosition(int X, int Y, out int ByteIndex, out int BitIndex)
        {
            if (!IsInside(X, Y))
            {
                throw new ArgumentOutOfRangeException($"Position [{X}, {Y}] is not inside (size={Size})");
            }

            int Linear = Y * Width + X;
            ByteIndex = Linear >> 3;
            BitIndex = Linear & Filter;
        }
        #endregion
    }
}