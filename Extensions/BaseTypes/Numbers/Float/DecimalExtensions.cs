namespace Zion
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Performs linear interpolation between two decimal values.
        /// </summary>
        /// <param name="A">The starting value.</param>
        /// <param name="B">The ending value.</param>
        /// <param name="Alpha">The interpolation factor (0-1).</param>
        /// <returns>The interpolated value between A and B.</returns>
        public static decimal Lerp(this decimal A, decimal B, decimal Alpha)
            => A + (B - A) * Alpha;

        /// <summary>
        /// Rounds the decimal value to the nearest integer using specified rounding mode.
        /// </summary>
        /// <param name="Value">The value to round.</param>
        /// <param name="Mode">The rounding mode to use.</param>
        /// <returns>The rounded integer value.</returns>
        public static int RoundToInt(this decimal Value, RoundMode Mode = RoundMode.Round)
            => (int)Round(Value, Mode);

        /// <summary>
        /// Rounds the decimal value using specified rounding mode.
        /// </summary>
        /// <param name="Value">The value to round.</param>
        /// <param name="Mode">The rounding mode to use.</param>
        /// <returns>The rounded value.</returns>
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