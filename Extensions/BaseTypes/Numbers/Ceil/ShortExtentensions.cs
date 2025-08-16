namespace Zion
{
    public static class ShortExtensions
    {
        /// <summary>
        /// Gets the state of a specific bit in the short integer.
        /// </summary>
        /// <param name="Value">The short integer value.</param>
        /// <param name="Index">The bit position (0-15).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        public static bool GetBit(this short Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        /// <summary>
        /// Sets or clears a specific bit in the short integer.
        /// </summary>
        /// <param name="Value">The short integer value.</param>
        /// <param name="Index">The bit position (0-15).</param>
        /// <param name="Bit">True to set the bit, false to clear it.</param>
        /// <returns>The modified short integer value.</returns>
        public static short SetBit(this short Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return (short)(Value | (1 << Index));
            }
            else
            {
                return (short)(Value & ~(1 << Index));
            }
        }

        /// <summary>
        /// Checks if the short integer value is even.
        /// </summary>
        /// <param name="Value">The short integer value.</param>
        /// <returns>True if the value is even, otherwise false.</returns>
        public static bool IsEven(this short Value)
        {
            return (Value & 1) == 0;
        }

        /// <summary>
        /// Gets the state of a specific bit in the unsigned short integer.
        /// </summary>
        /// <param name="Value">The unsigned short integer value.</param>
        /// <param name="Index">The bit position (0-15).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        public static bool GetBit(this ushort Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        /// <summary>
        /// Sets or clears a specific bit in the unsigned short integer.
        /// </summary>
        /// <param name="Value">The unsigned short integer value.</param>
        /// <param name="Index">The bit position (0-15).</param>
        /// <param name="Bit">True to set the bit, false to clear it.</param>
        /// <returns>The modified unsigned short integer value.</returns>
        public static ushort SetBit(this ushort Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return (ushort)(Value | (1 << Index));
            }
            else
            {
                return (ushort)(Value & ~(1 << Index));
            }
        }

        /// <summary>
        /// Checks if the unsigned short integer value is even.
        /// </summary>
        /// <param name="Value">The unsigned short integer value.</param>
        /// <returns>True if the value is even, otherwise false.</returns>
        public static bool IsEven(this ushort Value)
        {
            return (Value & 1) == 0;
        }
    }
}