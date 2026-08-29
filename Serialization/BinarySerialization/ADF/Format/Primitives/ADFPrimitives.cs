using Id = ushort;

namespace Zion.Serialization.ADF
{
    public static class ADFPrimitives
    {
        #region Info
        private static readonly PrimitiveInfo[] PrimitivesInfo =
        [
            new (sizeof(bool)),  //Boolean
            new (sizeof(byte)),  //Byte
            new (sizeof(sbyte)), //SByte

            new (sizeof(Int16 )), //Int16
            new (sizeof(Int32 )), //Int32
            new (sizeof(Int64 )), //Int64
            new (sizeof(UInt16)), //UInt16
            new (sizeof(UInt32)), //UInt32
            new (sizeof(UInt64)), //UInt64

            new (sizeof(char)),    //Char
            new (sizeof(float)),   //Single
            new (sizeof(double)),  //Double
            new (sizeof(decimal)), //Decimal
            new (2),               //String

            new (2),  //Half
            new (5),  //Index
            new (10), //Range
            new (-1), //BigInteger

            new (3), //RGB
            new (4), //RGBA

            new (8),  //Vector2
            new (8),  //Vector2Int
            new (12), //Vector3
            new (12), //Vector3Int

            new (4) //Reference;
        ];

        #endregion

        #region Ids
        public const int PrimitiveCount = 128;

        public const Id Boolean = 0;
        public const Id Byte    = 1;
        public const Id SByte   = 2;

        public const Id Int16   = 3;
        public const Id Int32   = 4;
        public const Id Int64   = 5;
        public const Id UInt16  = 6;
        public const Id UInt32  = 7;
        public const Id UInt64  = 8;

        public const Id Char    = 9;
        public const Id Single  = 10;
        public const Id Double  = 11;
        public const Id Decimal = 12;
        public const Id String  = 13;

        public const Id Half       = 14;
        public const Id Index      = 15;
        public const Id Range      = 16;
        public const Id BigInteger = 17;

        public const Id RGB  = 18;
        public const Id RGBA = 19;

        public const Id Vector2    = 20;
        public const Id Vector2Int = 21;
        public const Id Vector3    = 22;
        public const Id Vector3Int = 23;

        public const Id Reference = 24;

        #endregion

        #region PublicMethods
        public static bool IsPrimitive(Id Id)
        {
            return Id < PrimitiveCount;
        }

        public static int SizeOf(Id PrimitiveId)
        {
            return PrimitivesInfo[PrimitiveId].Size;
        }

        #endregion
    }
}