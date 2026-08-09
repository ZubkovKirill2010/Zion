namespace Zion.Serialization.TDC
{
    public enum PrimitiveTypes : byte
    {
        Boolean = 0,
        Byte = 1,
        SByte = 2,
        Char = 3,
        Decimal = 4,
        Double = 5,
        Single = 6,
        Int32 = 7,
        UInt32 = 8,
        Int64 = 9,
        UInt64 = 10,
        Int16 = 11,
        UInt16 = 12,
        String = 13,

        Half = 14,
        Index = 15,
        Range = 16,
        BigInteger = 17,

        RGB = 18,
        RGBA = 19,

        Vector2 = 20,
        Vector2Int = 21,
        Vector3 = 22,
        Vector3Int = 23
    }
}