using System.Numerics;
using BigInt = System.Numerics.BigInteger;

namespace Zion
{
    public sealed partial class ObjectReader //_Integers
    {
        private static readonly IntReaderData<byte> ByteData = new
        (
            true, true, 8, byte.MinValue, byte.MaxValue,
            (A, B) => (byte)(A + B),
            (A, B) => (byte)(A * B),
            (A, B) => (byte)(A / B),
            (A, B) => (byte)(A % B),
            (A, B) => (byte)(A << B),
            (A, B) => (byte)(A | B)
        );
        private static readonly IntReaderData<sbyte> SByteData = new
        (
            false, true, 7, sbyte.MinValue, sbyte.MaxValue,
            (A, B) => (sbyte)(A + B),
            (A, B) => (sbyte)(A * B),
            (A, B) => (sbyte)(A / B),
            (A, B) => (sbyte)(A % B),
            (A, B) => (sbyte)(A << B),
            (A, B) => (sbyte)((int)A | B)
        );

        private static readonly IntReaderData<BigInt> BigIntData = new
        (
            false, false, 0, BigInt.Zero, BigInt.Zero,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            (A, B) => A | B
        );

        private static readonly IntReaderData<short> Int16Data = new
        (
            false, true, 15, short.MinValue, short.MaxValue,
            (A, B) => (short)(A + B),
            (A, B) => (short)(A * B),
            (A, B) => (short)(A / B),
            (A, B) => (short)(A % B),
            (A, B) => (short)(A << B),
            (A, B) => (short)((int)A | B)
        );
        private static readonly IntReaderData<int> Int32Data = new
        (
            false, true, 31, int.MinValue, int.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => A % B,
            (A, B) => A << B,
            (A, B) => A | B
        );
        private static readonly IntReaderData<long> Int64Data = new
        (
            false, true, 63, long.MinValue, long.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            (A, B) => A | B
        );
        private static readonly IntReaderData<Int128> Int128Data = new
        (
            false, true, 127, Int128.MinValue, Int128.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            (A, B) => A | B
        );

        private static readonly IntReaderData<ushort> UInt16Data = new
        (
            true, true, 16, ushort.MinValue, ushort.MaxValue,
            (A, B) => (ushort)(A + B),
            (A, B) => (ushort)(A * B),
            (A, B) => (ushort)(A / B),
            (A, B) => (ushort)(A % B),
            (A, B) => (ushort)(A << B),
            (A, B) => (ushort)(A | B)
        );
        private static readonly IntReaderData<uint> UInt32Data = new
        (
            true, true, 32, uint.MinValue, uint.MaxValue,
            (A, B) => (uint)(A + B),
            (A, B) => (uint)(A * B),
            (A, B) => (uint)(A / B),
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            (A, B) => (uint)(A | B)
        );
        private static readonly IntReaderData<ulong> UInt64Data = new
        (
            true, true, 64, ulong.MinValue, ulong.MaxValue,
            (A, B) => A + (ulong)B,
            (A, B) => A * (ulong)B,
            (A, B) => A / (ulong)B,
            (A, B) => (int)(A % (ulong)B),
            (A, B) => A << B,
            (A, B) => A | (ulong)B
        );
        private static readonly IntReaderData<UInt128> UInt128Data = new
        (
            true, true, 128, UInt128.MinValue, UInt128.MaxValue,
            (A, B) => A + (UInt128)B,
            (A, B) => A * (UInt128)B,
            (A, B) => A / (UInt128)B,
            (A, B) => (int)(A % (UInt128)B),
            (A, B) => A << B,
            (A, B) => A | (UInt128)B
        );


        public bool TryReadByte(out byte Value)
        {
            return TryReadInt(ByteData, out Value);
        }

        public bool TryReadSByte(out sbyte Value)
        {
            return TryReadInt(SByteData, out Value);
        }


        public bool TryReadInt16(out short Value)
        {
            return TryReadInt(Int16Data, out Value);
        }

        public bool TryReadInt32(out int Value)
        {
            return TryReadInt(Int32Data, out Value);
        }

        public bool TryReadInt64(out long Value)
        {
            return TryReadInt(Int64Data, out Value);
        }

        public bool TryReadInt128(out Int128 Value)
        {
            return TryReadInt(Int128Data, out Value);
        }

        public bool TryReadBigInt(out BigInt Value)
        {
            return TryReadInt(BigIntData, out Value);
        }


        public bool TryReadUInt16(out ushort Value)
        {
            return TryReadInt(UInt16Data, out Value);
        }

        public bool TryReadUInt32(out uint Value)
        {
            return TryReadInt(UInt32Data, out Value);
        }

        public bool TryReadUInt64(out ulong Value)
        {
            return TryReadInt(UInt64Data, out Value);
        }

        public bool TryReadUInt128(out UInt128 Value)
        {
            return TryReadInt(UInt128Data, out Value);
        }



        public byte ReadByte()
        {
            return Unsafe<byte>(TryReadByte);
        }

        public sbyte ReadSByte()
        {
            return Unsafe<sbyte>(TryReadSByte);
        }


        public short ReadInt16()
        {
            return Unsafe<short>(TryReadInt16);
        }

        public int ReadInt32()
        {
            return Unsafe<int>(TryReadInt32);
        }

        public long ReadInt64()
        {
            return Unsafe<long>(TryReadInt64);
        }

        public Int128 ReadInt128()
        {
            return Unsafe<Int128>(TryReadInt128);
        }

        public BigInt ReadBigInt()
        {
            return Unsafe<BigInt>(TryReadBigInt);
        }


        public ushort ReadUInt16()
        {
            return Unsafe<ushort>(TryReadUInt16);
        }

        public uint ReadUInt32()
        {
            return Unsafe<uint>(TryReadUInt32);
        }

        public ulong ReadUInt64()
        {
            return Unsafe<ulong>(TryReadUInt64);
        }

        public UInt128 ReadUInt128()
        {
            return Unsafe<UInt128>(TryReadUInt128);
        }



        public bool TryReadInt<T>(IntReaderData<T> Data, out T Value) where T : IComparable<T>, INumber<T>, new()
        {
            Value = new();

            if (IsEnd) { return false; }

            int Index = this.Index;

            bool IsNegative = Text[Index] == '-';

            if (IsNegative)
            {
                if (Data.OnlyPositive)
                {
                    return false;
                }

                Index++;
            }


            if (Text.Begins(Index, "0x") || Text.Begins(Index, "0X"))
            {
                bool Result = TryReadBinaryInt(ref Value, Data, ref Index, IsNegative);

                if (Result) { this.Index = Index; }
                return Result;
            }
            else if (Text.Begins(Index, "0b") || Text.Begins(Index, "0B"))
            {
                bool Result = TryReadHexadecimalInt(ref Value, Data, ref Index, IsNegative);

                if (Result) { this.Index = Index; }
                return Result;
            }
            else
            {
                bool Result = TryReadDecimalInt(ref Value, Data, ref Index, IsNegative);

                if (Result) { this.Index = Index; }
                return Result;
            }
        }


        private bool TryReadBinaryInt<T>(ref T Value, IntReaderData<T> Data, ref int Index, bool IsNegative) where T : IComparable<T>, INumber<T>, new()
        {
            int Bits = 0;

            while (Index < Length)
            {
                char Char = Text[Index++];

                if (Char == '_') { continue; }
                if (Data.Suffixes.Contains(Char)) { break; }

                if (Char is '0' or '1')
                {
                    if (Bits++ == Data.BitCount) { return false; }

                    Value = Data.LeftShift(Value, 1);

                    if (Char == '1')
                    {
                        Value = Data.Or(Value, 1);
                    }
                }
                else
                {
                    return false;
                }
            }

            if (IsNegative)
            {
                Value = Data.Multiply(Value, -1);
            }

            return true;
        }

        private bool TryReadHexadecimalInt<T>(ref T Value, IntReaderData<T> Data, ref int Index, bool IsNegative) where T : IComparable<T>, INumber<T>, new()
        {
            while (Index < Length)
            {
                char Char = Text[Index++];

                if (Char == '_') { continue; }
                if (Data.Suffixes.Contains(Char)) { break; }

                if (IsHexadecimal(Char, out int Digit))
                {
                    if (IsNegative)
                    {
                        Digit = -Digit;
                    }

                    if (CheckOverflow(Value, Data, 16, Digit))
                    {
                        return false;
                    }

                    Value = Data.LeftShift(Value, 4);
                    Value = Data.Sum(Value, Digit);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool TryReadDecimalInt<T>(ref T Value, IntReaderData<T> Data, ref int Index, bool IsNegative) where T : IComparable<T>, INumber<T>, new()
        {
            while (Index < Length)
            {
                char Char = Text[Index++];

                if (Char == '_') { continue; }
                if (Data.Suffixes.Contains(Char)) { break; }

                if (IsDecimal(Char, out int Digit))
                {
                    if (IsNegative)
                    {
                        Digit = -Digit;
                    }

                    if (CheckOverflow(Value, Data, 10, Digit))
                    {
                        return false;
                    }

                    Value = Data.Multiply(Value, 10);
                    Value = Data.Sum(Value, Digit);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        private bool IsDecimal(char Char, out int Digit)
        {
            if (Char >= '0' && Char <= '9')
            {
                Digit = Char - '0';
                return true;
            }
            Digit = default;
            return false;
        }

        private bool IsHexadecimal(char Char, out int Digit)
        {
            if (IsDecimal(Char, out Digit))
            {
                return true;
            }

            if (Char >= 'a' && Char <= 'f')
            {
                Digit = Char - 'a';
                return true;
            }

            if (Char >= 'A' && Char <= 'F')
            {
                Digit = Char - 'A';
                return true;
            }

            return false;
        }


        private bool CheckOverflow<T>(T Value, IntReaderData<T> Data, int CalculusSystem, int Digit) where T : IComparable<T>, INumber<T>, new()
        {
            if (!Data.HasMaxValue)
            {
                return false;
            }

            T Zero = T.Zero;

            if (int.IsPositive(Value.CompareTo(Zero)))
            {
                if (int.IsNegative(Digit))
                {
                    return true;
                }

                T MaxDivision = Data.Divide(Data.MaxValue, CalculusSystem);

                if (Value.CompareTo(MaxDivision) > 0)
                {
                    return true;
                }

                if (Value.CompareTo(MaxDivision) == 0)
                {
                    int MaxRemainder = Data.Remained(Data.MaxValue, CalculusSystem);
                    if (Digit > MaxRemainder)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (int.IsPositive(Digit))
                {
                    return true;
                }

                T MinDivision = Data.Divide(Data.MinValue, CalculusSystem);

                if (Value.CompareTo(MinDivision) < 0)
                {
                    return true;
                }

                if (Value.CompareTo(MinDivision) == 0 && Digit < Data.Remained(Data.MinValue, CalculusSystem))
                {
                    return true;
                }
            }

            return false;
        }


        public readonly record struct IntReaderData<T>
        (
            bool OnlyPositive,
            bool HasMaxValue,
            int BitCount,

            T MinValue,
            T MaxValue,

            Func<T, int, T> Sum,
            Func<T, int, T> Multiply,
            Func<T, int, T> Divide,
            Func<T, int, int> Remained,
            Func<T, int, T> LeftShift,
            Func<T, int, T> Or,

            params IEnumerable<char> Suffixes
        )
            where T : IComparable<T>, INumber<T>;
    }
}