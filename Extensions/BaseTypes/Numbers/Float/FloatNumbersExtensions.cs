using System.Numerics;

namespace Zion
{
    public static class FloatNumbersExtensions
    {
        public static T Round<T>(this T Value, RoundMode Mode = RoundMode.Round) where T : IFloatingPointIeee754<T>
        {
            switch (Mode)
            {
                case RoundMode.Floor:
                    return T.Floor(Value);

                case RoundMode.Ceiling:
                    return T.Ceiling(Value);

                default:
                    return T.Round(Value);
            }
        }
    }
}