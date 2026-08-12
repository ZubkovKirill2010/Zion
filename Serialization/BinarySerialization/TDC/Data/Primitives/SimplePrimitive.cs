using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.Serialization.TDC
{
    public enum SimplePrimitive : byte
    {
        Boolean = 0,
        Byte    = 1,
        SByte   = 2,
        Char    = 3,
        Decimal = 4,
        Double  = 5,
        Single  = 6,
        Int32   = 7,
        UInt32  = 8,
        Int64   = 9,
        UInt64  = 10,
        Int16   = 11,
        UInt16  = 12,
        String  = 13,

        Half       = 14,
        Index      = 15,
        Range      = 16,
        BigInteger = 17,

        RGB  = 18,
        RGBA = 19,

        Vector2    = 20,
        Vector2Int = 21,
        Vector3    = 22,
        Vector3Int = 23
    }

    internal static class PrimitiveTypesExtensions
    {
        private const int _Count = 64;
        private static readonly Dictionary<Type, SimplePrimitive> PrimitivesInfo = new(24)
        {
            { typeof(bool), SimplePrimitive.Boolean },
            { typeof(byte), SimplePrimitive.Byte },
            { typeof(sbyte), SimplePrimitive.SByte },
            { typeof(char), SimplePrimitive.Char },
            { typeof(decimal), SimplePrimitive.Decimal },
            { typeof(double), SimplePrimitive.Double },
            { typeof(float), SimplePrimitive.Single },
            { typeof(int), SimplePrimitive.Int32 },
            { typeof(uint), SimplePrimitive.UInt32 },
            { typeof(long), SimplePrimitive.Int64 },
            { typeof(ulong), SimplePrimitive.UInt64 },
            { typeof(short), SimplePrimitive.Int16 },
            { typeof(ushort), SimplePrimitive.UInt16 },
            { typeof(string), SimplePrimitive.String },

            { typeof(Half), SimplePrimitive.Half },
            { typeof(Index), SimplePrimitive.Index },
            { typeof(Range), SimplePrimitive.Range },
            { typeof(BigInteger), SimplePrimitive.BigInteger },

            { typeof(RGBColor), SimplePrimitive.RGB },
            { typeof(RGBAColor), SimplePrimitive.RGBA },

            { typeof(Vector2), SimplePrimitive.Vector2 },
            { typeof(Vector2Int), SimplePrimitive.Vector2Int },
            { typeof(Vector3), SimplePrimitive.Vector3 },
            { typeof(Vector3Int), SimplePrimitive.Vector3Int }
        };

        extension(SimplePrimitive)
        {
            internal static int Count => _Count;

            public static bool Contains<T>(out SimplePrimitive Type)
            {
                return PrimitivesInfo.TryGetValue(typeof(T), out Type);
            }
        }
    }
}