namespace Zion
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Performs linear interpolation between two float values.
        /// </summary>
        /// <param name="A">The starting value.</param>
        /// <param name="B">The ending value.</param>
        /// <param name="Alpha">The interpolation factor (0-1).</param>
        /// <returns>The interpolated value between A and B.</returns>
        public static float Lerp(this float A, float B, float Alpha)
            => A + (B - A) * Alpha;

        /// <summary>
        /// Rounds the float value to the nearest integer using specified rounding mode.
        /// </summary>
        /// <param name="Value">The value to round.</param>
        /// <param name="Mode">The rounding mode to use.</param>
        /// <returns>The rounded integer value.</returns>
        public static int RoundToInt(this float Value, RoundMode Mode = RoundMode.Round)
            => (int)Value.Round(Mode);
    }
}