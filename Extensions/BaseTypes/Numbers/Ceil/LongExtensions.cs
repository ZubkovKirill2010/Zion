namespace Zion
{
    public static class LongExtensions
    {
        /// <summary>
        /// Gets the state of a specific bit in the long integer.
        /// </summary>
        /// <param name="Value">The long integer value.</param>
        /// <param name="Index">The bit position (0-63).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        public static bool GetBit(this long Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        /// <summary>
        /// Sets or clears a specific bit in the long integer.
        /// </summary>
        /// <param name="Value">The long integer value.</param>
        /// <param name="Index">The bit position (0-63).</param>
        /// <param name="Bit">True to set the bit, false to clear it.</param>
        /// <returns>The modified long integer value.</returns>
        public static long SetBit(this long Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return Value | (1 << Index);
            }
            else
            {
                return Value & ~(1 << Index);
            }
        }

        /// <summary>
        /// Checks if the long integer value is even.
        /// </summary>
        /// <param name="Value">The long integer value.</param>
        /// <returns>True if the value is even, otherwise false.</returns>
        public static bool IsEven(this long Value)
        {
            return (Value & 1) == 0;
        }

        /// <summary>
        /// Gets the state of a specific bit in the unsigned long integer.
        /// </summary>
        /// <param name="Value">The unsigned long integer value.</param>
        /// <param name="Index">The bit position (0-63).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        public static bool GetBit(this ulong Value, int Index)
        {
            return (Value & (ulong)(1 << Index)) != 0;
        }

        /// <summary>
        /// Sets or clears a specific bit in the unsigned long integer.
        /// </summary>
        /// <param name="Value">The unsigned long integer value.</param>
        /// <param name="Index">The bit position (0-63).</param>
        /// <param name="Bit">True to set the bit, false to clear it.</param>
        /// <returns>The modified unsigned long integer value.</returns>
        public static ulong SetBit(this ulong Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return Value | (ulong)(1 << Index);
            }
            else
            {
                return Value & ~(ulong)(1 << Index);
            }
        }

        /// <summary>
        /// Checks if the unsigned long integer value is even.
        /// </summary>
        /// <param name="Value">The unsigned long integer value.</param>
        /// <returns>True if the value is even, otherwise false.</returns>
        public static bool IsEven(this ulong Value)
        {
            return (Value & 1) == 0;
        }
    }
}