namespace Zion
{
    public static class FloatExtensions
    {
        public static float Lerp(this float A, float B, float Alpha)
        {
            return A + (B - A) * Alpha;
        }
    }
}