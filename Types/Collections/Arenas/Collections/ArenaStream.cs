using System.Numerics;
using System.Text;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion
{
    public sealed class ArenaStream : ArenaCollection<byte>
    {
        public int Length { get; private set; }

        public int Position
        {
            get;
            set
            {
                if (field == value) { return; }
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                field = value;
            }
        }


        public ArenaStream(ArenaSpan<byte> Data) : base(Data) { }


        public byte this[int Index]
        {
            get => Data[Index];
            set
            {
                Data[Index] = value;
                if (Index > Length)
                {
                    Length = Index + 1;
                }
            }
        }

        public byte this[Index Index]
        {
            get => this[Index.GetOffset(Length)];
            set => this[Index.GetOffset(Length)] = value;
        }


        public void Write(bool Value)
        {
            Write(Value ? (byte)1 : (byte)0);
        }

        public void Write(byte Value)
        {
            Reserve(1);
            this[Position++] = Value;
        }

        public void Write(sbyte Value)
        {
            Write((byte)Value);
        }

        public void Write(char Value)
        {
            Write((ushort)Value);
        }

        public void Write(decimal Value)
        {
            Span<int> Buffer = stackalloc int[4];
            decimal.GetBits(Value, Buffer);
            
            Reserve(sizeof(decimal));
            Write(Buffer[0]);
            Write(Buffer[1]);
            Write(Buffer[2]);
            Write(Buffer[3]);
        }

        public void Write(double Value)
        {
            Write(BitConverter.DoubleToUInt64Bits(Value));
        }

        public void Write(float Value)
        {
            Write(BitConverter.SingleToUInt32Bits(Value));
        }

        public void Write(int Value)
        {
            Write((uint)Value);
        }

        public void Write(uint Value)
        {
            Reserve(sizeof(uint));
            Write((byte)(Value >> 24));
            Write((byte)(Value >> 16));
            Write((byte)(Value >> 8));
            Write((byte)Value);
        }

        public void Write(long Value)
        {
            Write((ulong)Value);
        }

        public void Write(ulong Value)
        {
            Reserve(sizeof(ulong));
            Write((byte)(Value >> 56));
            Write((byte)(Value >> 48));
            Write((byte)(Value >> 40));
            Write((byte)(Value >> 32));
            Write((byte)(Value >> 24));
            Write((byte)(Value >> 16));
            Write((byte)(Value >> 8));
            Write((byte)Value);
        }

        public void Write(short Value)
        {
            Write((ushort)Value);
        }

        public void Write(ushort Value)
        {
            Reserve(sizeof(ushort));
            Write((byte)(Value >> 8));
            Write((byte)Value);
        }

        public void Write(string Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            int Length = Encoding.UTF8.GetByteCount(Value);
            
            Write7BitEncodedInt(Length);
            Reserve(Length);

            Encoding.UTF8.GetBytes(Value, Data.AsSpan(Position, Length));
            UpdateLengthFromPosition(Position + Length);
        }


        public void Write(Half Value)
        {
            Write(BitConverter.HalfToUInt16Bits(Value));
        }

        public void Write(Index Value)
        {
            Write(Value.IsFromEnd);
            Write(Value.Value);
        }

        public void Write(Range Value)
        {
            Write(Value.Start);
            Write(Value.End);
        }

        public void Write(BigInteger Value)
        {
            int Length = Value.GetByteCount();

            Reserve(Length + 4);
            Write(Length);

            Value.TryWriteBytes(Data.AsSpan(Position, Length), out _);
            UpdateLengthFromPosition(Position + Length);
        }


        public void Write(RGBColor Value)
        {
            Write(Value.R);
            Write(Value.G);
            Write(Value.B);
        }

        public void Write(RGBAColor Value)
        {
            Write(Value.R);
            Write(Value.G);
            Write(Value.B);
            Write(Value.A);
        }


        public void Write(Vector2 Value)
        {
            Write(Value.X);
            Write(Value.Y);
        }

        public void Write(Vector2Int Value)
        {
            Write(Value.X);
            Write(Value.Y);
        }

        public void Write(Vector3 Value)
        {
            Write(Value.X);
            Write(Value.Y);
            Write(Value.Z);
        }

        public void Write(Vector3Int Value)
        {
            Write(Value.X);
            Write(Value.Y);
            Write(Value.Z);
        }


        public void Write(Span<byte> Buffer)
        {
            Reserve(Buffer.Length);
            Buffer.CopyTo(Data.AsSpan(Position, Buffer.Length));
            UpdateLengthFromPosition(Position + Buffer.Length);
        }

        public void Write7BitEncodedInt(int Value)
        {
            uint UInt = (uint)Value;

            while (UInt >= 0x80)
            {
                Write((byte)(UInt | 0x80));
                UInt >>= 7;
            }

            Write((byte)UInt);
        }

        public void Write7BitEncodedInt64(long Value)
        {
            ulong UInt = (ulong)Value;

            while (UInt >= 0x80)
            {
                Write((byte)(UInt | 0x80));
                UInt >>= 7;
            }

            Write((byte)UInt);
        }


        public void Write(int Value, int Index)
        {
            int Start = Position;

            Position = Index;
            Write(Value);

            Position = Start;
        }


        public byte[] ToArray()
        {
            return Data.ToArray(0, Length);
        }

        public Span<byte> AsSpan()
        {
            return Data.AsSpan(0, Length);
        }


        public void CopyTo(Span<byte> Destination)
        {
            AsSpan().CopyTo(Destination);
        }

        public void CopyTo(Stream Stream)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            Stream.Write(AsSpan());
        }


        public void EnsureCapacity(int Capacity)
        {

        }

        public void Reserve(int Capacity)
        {
            EnsureCapacity(Position + Capacity);
        }


        public override IEnumerator<byte> GetEnumerator()
        {
            return Data.GetEnumerator(0, Length);
        }


        private void UpdateLengthFromPosition(int Position)
        {
            this.Position = Position;
            this.Length = Math.Max(this.Length, Position + 1);
        }
    }
}