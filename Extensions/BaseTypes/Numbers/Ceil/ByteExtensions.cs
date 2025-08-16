namespace Zion
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Gets the state of a specific bit in the byte.
        /// </summary>
        /// <param name="Value">The byte value.</param>
        /// <param name="Index">The bit position (0-7).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        public static bool GetBit(this byte Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        /// <summary>
        /// Sets or clears a specific bit in the byte.
        /// </summary>
        /// <param name="Value">The byte value.</param>
        /// <param name="Index">The bit position (0-7).</param>
        /// <param name="Bit">True to set the bit, false to clear it.</param>
        /// <returns>The modified byte value.</returns>
        public static byte SetBit(this byte Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return (byte)(Value | (1 << Index));
            }
            else
            {
                return (byte)(Value & ~(1 << Index));
            }
        }

        /// <summary>
        /// Checks if the byte value is even.
        /// </summary>
        /// <param name="Value">The byte value.</param>
        /// <returns>True if the value is even, otherwise false.</returns>
        public static bool IsEven(this byte Value)
        {
            return (Value & 1) == 0;
        }

        /// <summary>
        /// Gets the state of a specific bit in the sbyte.
        /// </summary>
        /// <param name="Value">The sbyte value.</param>
        /// <param name="Index">The bit position (0-7).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        public static bool GetBit(this sbyte Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        /// <summary>
        /// Sets or clears a specific bit in the sbyte.
        /// </summary>
        /// <param name="Value">The sbyte value.</param>
        /// <param name="Index">The bit position (0-7).</param>
        /// <param name="Bit">True to set the bit, false to clear it.</param>
        /// <returns>The modified sbyte value.</returns>
        public static sbyte SetBit(this sbyte Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return (sbyte)(Value | (sbyte)(1 << Index));
            }
            else
            {
                return (sbyte)(Value & ~(1 << Index));
            }
        }

        /// <summary>
        /// Checks if the sbyte value is even.
        /// </summary>
        /// <param name="Value">The sbyte value.</param>
        /// <returns>True if the value is even, otherwise false.</returns>
        public static bool IsEven(this sbyte Value)
        {
            return (Value & 1) == 0;
        }
    }
}