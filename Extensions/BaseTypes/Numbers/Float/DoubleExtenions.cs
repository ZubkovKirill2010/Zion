namespace Zion
{
    public static class DoubleExtensions
    {
        public static double Lerp(this double A, double B, double Alpha)
        {
            return A + (B - A) * Alpha;
        }
    }
}