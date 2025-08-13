using Zion.Vectors;

namespace Zion
{
    public sealed class BitMatrix : IBinaryObject<BitMatrix>
    {
        private readonly byte[] Data;
        public readonly Vector2Int Size;

        public BitMatrix(Vector2Int Size)
        {
            Data = new byte[(int)Math.Ceiling((float)(Size.x * Size.y / 8))];
            this.Size = Size;
        }
        public BitMatrix(int Width, int Height)
        {
            Data = new byte[(int)Math.Ceiling((float)(Width * Height / 8))];
            Size = new Vector2Int(Width, Height);
        }
        public BitMatrix(int SizeLength)
        {
            Data = new byte[(int)Math.Ceiling((float)(SizeLength * SizeLength / 8))];
            Size = new Vector2Int(SizeLength);
        }
        private BitMatrix(byte[] Data, Vector2Int Size)
        {
            this.Data = Data;
            this.Size = Size;
        }
        private BitMatrix(Vector2Int Size, int Count, Func<byte> GetValue)
        {
            Data = new byte[Count];
            this.Size = Size;

            for (int i = 0; i < Count; i++)
            {
                Data[i] = GetValue();
            }
        }

        public bool this[int x, int y]
        {
            get
            {
                (int, int) Position = GetPosition(x, y);
                return Data[Position.Item1].GetBit(Position.Item2);
            }
            set
            {
                (int, int) Position = GetPosition(x, y);
                Data[Position.Item1].SetBit(Position.Item2, value);
            }
        }
        public bool this[Vector2Int Position]
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

        public BitMatrix Clone()
        {
            return new BitMatrix(ArrayExtensions.Clone(Data), Size);
        }
        public BitMatrix GetSubMatrix(Vector2Int Start, Vector2Int End)
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
            BitMatrix Result = new BitMatrix(MatrixSize);

            for (int x = 0; x < MatrixSize.x; x++)
            {
                for (int y = 0; y < MatrixSize.y; y++)
                {
                    Result[x, y] = this[Start.x + x, Start.y + y];
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
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Action(this[x, y]);
                }
            }
        }
        public void ForEach(Func<bool, bool> Converter)
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    this[x, y] = Converter(this[x, y]);
                }
            }
        }

        public Matrix<I> Convert<I>(Converter<bool, I> Converter)
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

        public void Reverse()
        {
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = (byte)~Data[i];
            }
        }

        private (int, int) GetPosition(int x, int y)
        {
            if (!IsInside(x, y))
            {
                throw new ArgumentOutOfRangeException($"Position [{x}, {y}] is not inside (size={Size})");
            }
            int BitIndex = Size.x * y + x;
            return (BitIndex / 8, BitIndex % 8);
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Size);
            Writer.Write(Data.Length);
            foreach (byte Item in Data)
            {
                Writer.Write(Item);
            }
        }
        public static BitMatrix Read(BinaryReader Reader)
        {
            return new BitMatrix
            (
                Reader.Read<Vector2Int>(),
                Reader.ReadInt32(),
                Reader.ReadByte
            );
        }
    }
}