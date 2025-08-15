namespace Zion
{
    public static class DecimalExtensions
    {
        public static decimal Lerp(this decimal A, decimal B, decimal Alpha)
            => A + (B - A) * Alpha;

        public static int RoundToInt(this decimal Value, RoundMode Mode = RoundMode.Round)
            => (int)Round(Value, Mode);

        public static decimal Round(this decimal Value, RoundMode Mode = RoundMode.Round)
        {
            switch (Mode)
            {
                case RoundMode.Floor:
                    return Math.Floor(Value);

                case RoundMode.Ceiling:
                    return Math.Ceiling(Value);

                default:
                    return Math.Round(Value);
            }
        }
    }
}