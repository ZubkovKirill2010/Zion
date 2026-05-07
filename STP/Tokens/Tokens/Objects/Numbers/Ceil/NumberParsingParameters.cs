using System.Numerics;

namespace Zion.STP
{
    public readonly record struct NumberParsingParameters<T>
    (
        bool OnlyPositive,
        bool HasMaxValue,
        int BitCount,

        T MinValue,
        T MaxValue,

        Func<T, int, T> Sum,
        Func<T, int, T> Multiply,
        Func<T, int, T> Divide,
        Func<T, int, int> Remainder,
        Func<T, int, T> LeftShift,
        Func<T, int, T> Or,

        params char[] Suffixes
    ) where T : IComparable<T>, INumber<T>
    {
        public static readonly NumberParsingParameters<byte> Byte = new
        (
            true, true, 8, byte.MinValue, byte.MaxValue,
            (A, B) => (byte)(A + B),
            (A, B) => (byte)(A * B),
            (A, B) => (byte)(A / B),
            (A, B) => (byte)(A % B),
            (A, B) => (byte)(A << B),
            (A, B) => (byte)(A | B)
        );
        public static readonly NumberParsingParameters<sbyte> SByte = new
        (
            false, true, 7, sbyte.MinValue, sbyte.MaxValue,
            (A, B) => (sbyte)(A + B),
            (A, B) => (sbyte)(A * B),
            (A, B) => (sbyte)(A / B),
            (A, B) => (sbyte)(A % B),
            (A, B) => (sbyte)(A << B),
            (A, B) => (sbyte)(A | B)
        );

        public static readonly NumberParsingParameters<BigInteger> BigInt = new
        (
            false, false, 0, BigInteger.Zero, BigInteger.Zero,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            (A, B) => A | B
        );

        public static readonly NumberParsingParameters<short> Int16 = new
        (
            false, true, 15, short.MinValue, short.MaxValue,
            (A, B) => (short)(A + B),
            (A, B) => (short)(A * B),
            (A, B) => (short)(A / B),
            (A, B) => (short)(A % B),
            (A, B) => (short)(A << B),
            (A, B) => (short)(A | B)
        );
        public static readonly NumberParsingParameters<int> Int32 = new
        (
            false, true, 31, int.MinValue, int.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => A % B,
            (A, B) => A << B,
            (A, B) => A | B
        );
        public static readonly NumberParsingParameters<long> Int64 = new
        (
            false, true, 63, long.MinValue, long.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            (A, B) => A | B
        );
        public static readonly NumberParsingParameters<Int128> Int128 = new
        (
            false, true, 127, Int128.MinValue, Int128.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            (A, B) => A | B
        );

        public static readonly NumberParsingParameters<ushort> UInt16 = new
        (
            true, true, 16, ushort.MinValue, ushort.MaxValue,
            (A, B) => (ushort)(A + B),
            (A, B) => (ushort)(A * B),
            (A, B) => (ushort)(A / B),
            (A, B) => (ushort)(A % B),
            (A, B) => (ushort)(A << B),
            (A, B) => (ushort)(A | B)
        );
        public static readonly NumberParsingParameters<uint> UInt32 = new
        (
            true, true, 32, uint.MinValue, uint.MaxValue,
            (A, B) => (uint)(A + B),
            (A, B) => (uint)(A * B),
            (A, B) => (uint)(A / B),
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            (A, B) => (uint)(A | B)
        );
        public static readonly NumberParsingParameters<ulong> UInt64 = new
        (
            true, true, 64, ulong.MinValue, ulong.MaxValue,
            (A, B) => A + (ulong)B,
            (A, B) => A * (ulong)B,
            (A, B) => A / (ulong)B,
            (A, B) => (int)(A % (ulong)B),
            (A, B) => A << B,
            (A, B) => A | (ulong)B
        );
        public static readonly NumberParsingParameters<UInt128> UInt128 = new
        (
            true, true, 128, UInt128.MinValue, UInt128.MaxValue,
            (A, B) => A + (UInt128)B,
            (A, B) => A * (UInt128)B,
            (A, B) => A / (UInt128)B,
            (A, B) => (int)(A % (UInt128)B),
            (A, B) => A << B,
            (A, B) => A | (UInt128)B
        );
    }
}