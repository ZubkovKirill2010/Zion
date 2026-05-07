using System.Numerics;

namespace Zion.STP
{
    public readonly struct ByteToken : IValueToken<byte>
    {
        public int Length { get; init; }
        public byte Value { get; init; }
        public TokenStatus Status { get; init; }
    }

    public readonly struct SByteToken : IValueToken<sbyte>
    {
        public int Length { get; init; }
        public sbyte Value { get; init; }
        public TokenStatus Status { get; init; }
    }


    public readonly struct Int16Token : IValueToken<short>
    {
        public int Length { get; init; }
        public short Value { get; init; }
        public TokenStatus Status { get; init; }
    }

    public readonly struct Int32Token : IValueToken<int>
    {
        public int Length { get; init; }
        public int Value { get; init; }
        public TokenStatus Status { get; init; }
    }

    public readonly struct Int64Token : IValueToken<long>
    {
        public int Length { get; init; }
        public long Value { get; init; }
        public TokenStatus Status { get; init; }
    }

    public readonly struct Int128Token : IValueToken<Int128>
    {
        public int Length { get; init; }
        public Int128 Value { get; init; }
        public TokenStatus Status { get; init; }
    }


    public readonly struct UInt16Token : IValueToken<ushort>
    {
        public int Length { get; init; }
        public ushort Value { get; init; }
        public TokenStatus Status { get; init; }
    }

    public readonly struct UInt32Token : IValueToken<uint>
    {
        public int Length { get; init; }
        public uint Value { get; init; }
        public TokenStatus Status { get; init; }
    }

    public readonly struct UInt64Token : IValueToken<ulong>
    {
        public int Length { get; init; }
        public ulong Value { get; init; }
        public TokenStatus Status { get; init; }
    }

    public readonly struct UInt128Token : IValueToken<UInt128>
    {
        public int Length { get; init; }
        public UInt128 Value { get; init; }
        public TokenStatus Status { get; init; }
    }


    public readonly struct BigIntToken : IValueToken<BigInteger>
    {
        public int Length { get; init; }
        public BigInteger Value { get; init; }
        public TokenStatus Status { get; init; }
    }
}