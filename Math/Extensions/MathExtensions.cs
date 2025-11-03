using Int = System.Numerics.BigInteger;

namespace Zion.MathExpressions
{
    public static class MathExtensions
    {
        public static Fraction ToFraction(this Int Int)
        {
            return new Fraction(Int);
        }
    }
}