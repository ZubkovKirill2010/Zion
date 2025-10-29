using System.Text;

namespace Zion.Diagnostics
{
    public sealed class TestBinaryWriter : BinaryWriter
    {
        private readonly bool Writing;
        private StringBuilder Builder = new StringBuilder();
        private Encoding Encoding;

        public TestBinaryWriter(bool Writing, Stream Output)
            : this(Writing, Output, Encoding.UTF8, false) { }

        public TestBinaryWriter(bool Writing, Stream Output, Encoding Encoding)
            : this(Writing, Output, Encoding, false) { }

        public TestBinaryWriter(bool Writing, Stream Output, Encoding Encoding, bool LeaveOpen)
            : base(Output, Encoding, LeaveOpen)
        {
            this.Writing = Writing;
            this.Encoding = Encoding;
        }


        public override string ToString() => Builder.ToString();


        public override long Seek(int Offset, SeekOrigin Origin)
        {
            Out($"Seek(Offset: {Offset}, {Origin})");
            return base.Seek(Offset, Origin);
        }


        public override void Write(ulong Value)
        {
            Out<ulong>(64, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(uint Value)
        {
            Out<uint>(32, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(ushort Value)
        {
            Out<ushort>(16, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(string Value)
        {
            int size = Value != null ? 8 * Encoding.GetByteCount(Value) : 0;
            Out<string>(size, Value ?? "null");
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(float Value)
        {
            Out<float>(32, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(sbyte Value)
        {
            Out<sbyte>(8, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(ReadOnlySpan<char> Chars)
        {
            int size = 8 * Encoding.GetByteCount(Chars);
            OutReadOnlySpanChar(size, Chars);
            base.Write(Chars);
        }

        public override void Write(ReadOnlySpan<byte> Buffer)
        {
            OutReadOnlySpanByte(Buffer.Length * 8, Buffer);
            base.Write(Buffer);
        }

        public override void Write(char[] Chars, int Index, int Count)
        {
            if (Chars == null)
            {
                Out<char[]>(0, "null");
                base.Write(Chars, Index, Count);
                return;
            }

            int size = 8 * Encoding.GetByteCount(Chars, Index, Count);
            string content = Count > 50
                ? $"Array[Length: {Chars.Length}], Index: {Index}, Count: {Count}, Content: \"{new string(Chars, Index, Math.Min(50, Count))}...\""
                : $"Array[Length: {Chars.Length}], Index: {Index}, Count: {Count}, Content: \"{new string(Chars, Index, Count)}\"";

            Out<char[]>(size, content);
            base.Write(Chars, Index, Count);
        }

        public override void Write(byte[] Buffer, int Index, int Count)
        {
            if (Buffer == null)
            {
                Out<byte[]>(0, "null");
                base.Write(Buffer, Index, Count);
                return;
            }

            string bytesInfo = Count > 20
                ? $"Array[Length: {Buffer.Length}], Index: {Index}, Count: {Count}, First 20 bytes: {BitConverter.ToString(Buffer, Index, Math.Min(20, Count))}..."
                : $"Array[Length: {Buffer.Length}], Index: {Index}, Count: {Count}, Bytes: {BitConverter.ToString(Buffer, Index, Count)}";

            Out<byte[]>(Count * 8, bytesInfo);
            base.Write(Buffer, Index, Count);
        }



        public override void Write(long Value)
        {
            Out<long>(64, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(int Value)
        {
            Out<int>(32, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(Half Value)
        {
            Out<Half>(16, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(double Value)
        {
            Out<double>(64, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(decimal Value)
        {
            Out<decimal>(128, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(char Value)
        {
            int size = 8 * Encoding.GetByteCount([Value]);
            Out<char>(size, $"'{Value}' (Unicode: {(int)Value:X4})");
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(byte[] Buffer)
        {
            int size = Buffer?.Length * 8 ?? 0;
            string binaryRepresentation = Buffer != null
                ? $"[{string.Join(' ', Buffer.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')))}]"
                : "null";

            Out<byte[]>(size, $"Length: {Buffer?.Length ?? 0}, Binary: {binaryRepresentation}");
            if (Writing)
            {
                base.Write(Buffer);
            }
        }

        public override void Write(byte Value)
        {
            Out<byte>(8, $"{Value} (0x{Value:X2}, binary: {Convert.ToString(Value, 2).PadLeft(8, '0')})");
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(bool Value)
        {
            Out<bool>(8, $"{Value} ({(Value ? 1 : 0)})");
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(short Value)
        {
            Out<short>(16, Value);
            if (Writing)
            {
                base.Write(Value);
            }
        }

        public override void Write(char[] Chars)
        {
            int size = Chars != null ? 8 * Encoding.GetByteCount(Chars) : 0;
            Out<char[]>(size, $"Array[Length: {Chars?.Length ?? 0}], Content: \"{(Chars != null ? new string(Chars) : "null")}\"");
            if (Writing)
            {
                base.Write(Chars);
            }
        }


        public new void Write7BitEncodedInt(int Value)
        {
            // Реализация 7-bit encoded int
            uint num = (uint)Value;
            while (num >= 0x80)
            {
                Write((byte)(num | 0x80));
                num >>= 7;
            }
            Write((byte)num);

            Out<int>(-1, $"7BitEncodedInt: {Value} (variable length)");
        }

        public new void Write7BitEncodedInt64(long Value)
        {
            // Реализация 7-bit encoded long
            ulong num = (ulong)Value;
            while (num >= 0x80)
            {
                Write((byte)(num | 0x80));
                num >>= 7;
            }
            Write((byte)num);

            Out<long>(-1, $"7BitEncodedInt64: {Value} (variable length)");
        }


        private void Out(object? Message)
        {
            Builder.AppendLine(Message.ToNotNullString());
        }

        private void Out<T>(int Size, object? Message)
        {
            string sizeStr = Size >= 0 ? $"{Size}" : "variable";
            Builder.AppendLine($"{sizeStr} | {typeof(T).Name} | {Message.ToNotNullString()}");
        }

        private void OutReadOnlySpanChar(int Size, ReadOnlySpan<char> Chars)
        {
            Builder.AppendLine($"{Size} | ReadOnlySpan<char> | Length: {Chars.Length}");
        }

        private void OutReadOnlySpanByte(int Size, ReadOnlySpan<byte> Buffer)
        {
            Builder.AppendLine($"{Size} | ReadOnlySpan<byte> | Length: {Buffer.Length}");
        }
    }
}