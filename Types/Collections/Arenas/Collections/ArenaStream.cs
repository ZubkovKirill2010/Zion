using System.Numerics;
using System.Text;
using System.Runtime.InteropServices;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;
using static System.Buffers.Binary.BinaryPrimitives;

namespace Zion
{
    public sealed class ArenaStream : ArenaCollection<byte>
    {
        public int Length { get; private set; }

        private int _Position;
        public int Position
        {
            get => _Position;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                _Position = value;
            }
        }


        public ArenaStream(ArenaSpan<byte> Data) : base(Data) { }


        public byte this[int Index]
        {
            get => Data[Index];
            set
            {
                var Data = this.Data;
                Data[Index] = value;
                if (Index >= Length)
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
            this[Position] = Value;
            UpdateLengthFromPosition(Position + 1);
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
            Reserve(sizeof(decimal));
            Data.Use
            (
                _Position,
                Span =>
                {
                    Span<int> Destination = MemoryMarshal.Cast<byte, int>(Span);
                    decimal.GetBits(Value, Destination);
                    UpdateLengthFromPosition(_Position + sizeof(decimal));
                }
            );
        }

        public void Write(double Value)
        {
            Write(Value, sizeof(double), WriteDoubleLittleEndian);
        }

        public void Write(float Value)
        {
            Write(Value, sizeof(float), WriteSingleLittleEndian);
        }

        public void Write(int Value)
        {
            Write(Value, sizeof(int), WriteInt32LittleEndian);
        }

        public void Write(uint Value)
        {
            Write(Value, sizeof(uint), WriteUInt32LittleEndian);
        }

        public void Write(long Value)
        {
            Write(Value, sizeof(long), WriteInt64LittleEndian);
        }

        public void Write(ulong Value)
        {
            Write(Value, sizeof(ulong), WriteUInt64LittleEndian);
        }

        public void Write(short Value)
        {
            Write(Value, sizeof(short), WriteInt16LittleEndian);
        }

        public void Write(ushort Value)
        {
            Write(Value, sizeof(ushort), WriteUInt16LittleEndian);
        }

        public void Write(string Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            int Length = Encoding.UTF8.GetByteCount(Value);
            
            Write7BitEncodedInt(Length);
            Reserve(Length);

            Data.Use
            (
                _Position,
                Span => Encoding.UTF8.GetBytes(Value, Span)
            );

            UpdateLengthFromPosition(Position + Length);
        }


        public void Write(Half Value)
        {
            Write(Value, 2, WriteHalfLittleEndian);
        }

        public void Write(Index Value)
        {
            Reserve(sizeof(bool) + sizeof(int));
            WriteIndex(Value);
        }

        public void Write(Range Value)
        {
            Reserve(2 * (sizeof(bool) + sizeof(int)));
            WriteIndex(Value.Start);
            WriteIndex(Value.End);
        }

        public void Write(BigInteger Value)
        {
            int Position = _Position;
            int Length = Value.GetByteCount();

            Reserve(Length + 4);
            Data.Use
            (
                _Position,
                Span =>
                {
                    WriteInt32LittleEndian(Span, Length);
                    Value.TryWriteBytes(Span.Slice(4), out _);   
                }
            );
            UpdateLengthFromPosition(Position + Length + 4);
        }


        public void Write(RGBColor Value)
        {
            Reserve(3);
            Data.Use
            (
                _Position,
                Span =>
                {
                    Span[0] = Value.R;
                    Span[1] = Value.G;
                    Span[2] = Value.B;
                }
            );
            UpdateLengthFromPosition(_Position + 3);
        }

        public void Write(RGBAColor Value)
        {
            Reserve(4);
            Data.Use
            (
                _Position,
                Span =>
                {
                    Span[0] = Value.R;
                    Span[1] = Value.G;
                    Span[2] = Value.B;
                    Span[3] = Value.A;
                }
            );
            UpdateLengthFromPosition(_Position + 4);
        }


        public void Write(Vector2 Value)
        {
            Reserve(8);
            Data.Use
            (
                _Position,
                Span =>
                {
                    WriteSingleLittleEndian(Span, Value.X);
                    WriteSingleLittleEndian(Span.Slice(4), Value.Y);
                }
            );
            UpdateLengthFromPosition(_Position + 8);
        }

        public void Write(Vector2Int Value)
        {
            Reserve(8);
            Data.Use
            (
                _Position,
                Span =>
                {
                    WriteInt32LittleEndian(Span, Value.X);
                    WriteInt32LittleEndian(Span.Slice(4), Value.Y);
                }
            );
            UpdateLengthFromPosition(_Position + 8);
        }

        public void Write(Vector3 Value)
        {
            Reserve(12);
            Data.Use
            (
                _Position,
                Span =>
                {
                    WriteSingleLittleEndian(Span, Value.X);
                    WriteSingleLittleEndian(Span.Slice(4), Value.Y);
                    WriteSingleLittleEndian(Span.Slice(8), Value.Z);
                }
            );
            UpdateLengthFromPosition(_Position + 12);
        }

        public void Write(Vector3Int Value)
        {
            Reserve(12);
            Data.Use
            (
                _Position,
                Span =>
                {
                    WriteInt32LittleEndian(Span, Value.X);
                    WriteInt32LittleEndian(Span.Slice(4), Value.Y);
                    WriteInt32LittleEndian(Span.Slice(8), Value.Z);
                }
            );
            UpdateLengthFromPosition(_Position + 12);
        }


        public void Write7BitEncodedInt(int Value)
        {
            Write7BitEncodedUInt((uint)((Value << 1) ^ (Value >> 31)));
        }

        public void Write7BitEncodedInt64(long Value)
        {
            Write7BitEncodedUInt64((ulong)((Value << 1) ^ (Value >> 63)));
        }

        public void Write7BitEncodedUInt(uint Value)
        {
            Reserve(5);
            var Index = 0;

            Data.Use
            (
                _Position,
                Span =>
                {
                    while (Value >= 0x80)
                    {
                        Span[Index++] = (byte)(Value | 0x80);
                        Value >>= 7;
                    }
                    Span[Index++] = (byte)Value;
                }
            );

            UpdateLengthFromPosition(_Position + Index);
        }

        public void Write7BitEncodedUInt64(ulong Value)
        {
            Reserve(10);
            var Index = 0;

            Data.Use
            (
                _Position,
                Span =>
                {
                    while (Value >= 0x80)
                    {
                        Span[Index++] = (byte)(Value | 0x80);
                        Value >>= 7;
                    }
                    Span[Index++] = (byte)Value;
                }
            );

            UpdateLengthFromPosition(_Position + Index);
        }

        public void Write7BitEncodedIndex(Index Value)
        {
            uint Encoded = (uint)((Value.Value << 1) ^ (Value.Value >> 31));

            Reserve(5);
            var Index = 0;

            Data.Use
            (
                _Position,
                Span =>
                {
                    byte FirstByte = (byte)(Encoded & 0x3F);
                    Encoded >>= 6;

                    if (Value.IsFromEnd)
                    {
                        FirstByte |= 0x40;
                    }

                    if (Encoded > 0)
                    {
                        FirstByte |= 0x80;
                    }

                    Span[Index++] = FirstByte;

                    while (Encoded > 0)
                    {
                        if (Encoded >= 0x80)
                        {
                            Span[Index++] = (byte)(Encoded | 0x80);
                            Encoded >>= 7;
                        }
                        else
                        {
                            Span[Index++] = (byte)Encoded;
                            break;
                        }
                    }
                }
            );

            UpdateLengthFromPosition(_Position + Index);
        }


        private void Write<T>(T Value, int Size, Action<Span<byte>, T> Write)
        {
            Reserve(Size);
            Data.Use
            (
                _Position,
                Span =>
                {
                    Write(Span, Value);
                }
            );
            UpdateLengthFromPosition(_Position + Size);
        }

        private void WriteIndex(Index Value)
        {
            this[_Position++] = Value.IsFromEnd ? (byte)1 : (byte)0;
            Write(Value.Value, sizeof(int), WriteInt32LittleEndian);
        }


        public byte[] ToArray()
        {
            return Data.ToArray(0, Length);
        }


        public void CopyTo(Span<byte> Destination)
        {
            Data.CopyTo(0, Length, Destination);
        }

        public void CopyTo(Stream Stream)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            Data.Use(Span => Stream.Write(Span.Slice(0, Length)));
        }


        public void Reserve(int Size)
        {
            int Required = _Position + Size;

            if (Required > Data.Count)
            {
                int NewSize = Data.Count < 256
                    ? Math.Max(Data.Count * 2, Required)
                    : Math.Max(Data.Count + 256, Required);

                Expand(NewSize);
            }
        }


        public override IEnumerator<byte> GetEnumerator()
        {
            //TODO
            throw new NotImplementedException();
        }


        private void UpdateLengthFromPosition(int Position)
        {
            _Position = Position;
            if (Position > Length)
            {
                Length = Position;
            }
        }
    }
}