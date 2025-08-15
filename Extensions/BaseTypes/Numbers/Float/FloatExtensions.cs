namespace Zion
{
    public static class FloatExtensions
    {
        public static float Lerp(this float A, float B, float Alpha)
            => A + (B - A) * Alpha;

        public static int RoundToInt(this float Value, RoundMode Mode = RoundMode.Round)
            => (int)Value.Round(Mode);
    }
}