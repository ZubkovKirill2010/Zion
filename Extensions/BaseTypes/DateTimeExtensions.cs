namespace Zion
{
    public record struct MonthInfo(string Name, int DaysCount);

    public static class DateTimeExtension
    {
        private static readonly MonthInfo[] _Months =
        [
            new MonthInfo("January", 31),
            new MonthInfo("February", 28),
            new MonthInfo("March", 31),
            new MonthInfo("April", 30),
            new MonthInfo("May", 31),
            new MonthInfo("June", 30),
            new MonthInfo("July", 31),
            new MonthInfo("August", 31),
            new MonthInfo("September", 30),
            new MonthInfo("October", 31),
            new MonthInfo("November", 30),
            new MonthInfo("December", 31)
        ];

        /// <summary>
        /// Gets month information by index.
        /// </summary>
        /// <param name="Index">The month index (1-12).</param>
        /// <param name="LeapYear">Whether to account for leap year for February.</param>
        /// <returns>Month information including name and day count.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
        public static MonthInfo GetMonthInfo(int Index, bool LeapYear = false)
        {
            if (!Index.IsClamp(1, 12))
            {
                throw new ArgumentOutOfRangeException($"The month index must be between 1 and 12, InputIndex = {Index}");
            }

            if (LeapYear && Index == 2 && (DateTime.Now.Year & 3) == 0)
            {
                return _Months[Index] with { DaysCount = 29 };
            }

            return _Months[Index - 1];
        }

        /// <summary>
        /// Gets month information from a DateTime object.
        /// </summary>
        /// <param name="Date">The DateTime object to examine.</param>
        /// <param name="LeapYear">Whether to account for leap year for February.</param>
        /// <returns>Month information including name and day count.</returns>
        public static MonthInfo GetMonthInfo(this DateTime Date, bool LeapYear = false)
        {
            return GetMonthInfo(Date.Month, LeapYear);
        }

        /// <summary>
        /// Gets the current date as a DateOnly object.
        /// </summary>
        /// <returns>Current date in DateOnly format.</returns>
        public static DateOnly GetNow()
        {
            DateTime Now = DateTime.Today;
            return new DateOnly(Now.Year, Now.Month, Now.Day);
        }
    }
}