using System.Numerics;

namespace Zion.STP
{
    public sealed class ByteToken : ValueToken<byte> { }

    public sealed class SByteToken : ValueToken<sbyte> { }


    public sealed class Int16Token : ValueToken<short> { }

    public sealed class Int32Token : ValueToken<int> { }

    public sealed class Int64Token : ValueToken<long> { }

    public sealed class Int128Token : ValueToken<Int128> { }


    public sealed class UInt16Token : ValueToken<ushort> { }

    public sealed class UInt32Token : ValueToken<uint> { }

    public sealed class UInt64Token : ValueToken<ulong> { }

    public sealed class UInt128Token : ValueToken<UInt128> { }


    public sealed class BigIntToken : ValueToken<BigInteger> { }
}