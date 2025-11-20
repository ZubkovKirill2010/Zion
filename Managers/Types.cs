using System.Numerics;
using Zion.MathExpressions;

namespace Zion
{
    public static class Types
    {
        public static readonly Type Byte = typeof(byte);
        public static readonly Type Int16 = typeof(short);
        public static readonly Type Int32 = typeof(int);
        public static readonly Type Int64 = typeof(long);
        public static readonly Type Int128 = typeof(Int128);

        public static readonly Type SByte = typeof(sbyte);
        public static readonly Type UInt16 = typeof(uint);
        public static readonly Type UInt32 = typeof(uint);
        public static readonly Type UInt64 = typeof(ulong);
        public static readonly Type UInt128 = typeof(UInt128);

        public static readonly Type Fraction = typeof(Fraction);
        public static readonly Type BigInteger = typeof(BigInteger);

        public static readonly Type Float = typeof(float);
        public static readonly Type Double = typeof(double);

        public static readonly Type String = typeof(string);
        public static readonly Type Boolean = typeof(bool);
    }
}