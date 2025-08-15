namespace Zion
{
    public static class DoubleExtensions
    {
        public static double Lerp(this double A, double B, double Alpha)
            => A + (B - A) * Alpha;

        public static int RoundToInt(this double Value, RoundMode Mode = RoundMode.Round)
            => (int)Value.Round(Mode);
    }
}