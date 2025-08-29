namespace Zion
{
    public static class IntExtensions
    {
        /// <summary>
        /// Gets the state of a specific bit in the integer.
        /// </summary>
        /// <param name="Value">The integer value.</param>
        /// <param name="Index">The bit position (0-31).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        public static bool GetBit(this int Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        /// <summary>
        /// Sets or clears a specific bit in the integer.
        /// </summary>
        /// <param name="Value">The integer value.</param>
        /// <param name="Index">The bit position (0-31).</param>
        /// <param name="Bit">True to set the bit, false to clear it.</param>
        /// <returns>The modified integer value.</returns>
        public static int SetBit(this int Value, int Index, bool Bit)
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
        /// Checks if the integer value is even.
        /// </summary>
        /// <param name="Value">The integer value.</param>
        /// <returns>True if the value is even, otherwise false.</returns>
        public static bool IsEven(this int Value)
        {
            return (Value & 1) == 0;
        }

        public static bool IsPrime(this int Value)
        {
            if (Value <= 1) { return false; }
            if (Value == 2) { return true; }
            if (IsEven(Value)) { return false; }

            int MaxValue = (int)Math.Sqrt(Value);

            for (int i = 3; i <= MaxValue; i += 2)
            {
                if (Value % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsInRange<T>(this int Value, ICollection<T> Collection)
        {
            ArgumentNullException.ThrowIfNull(Collection);

            return Value >= 0 && Value < Collection.Count;
        }


        /// <summary>
        /// Gets the state of a specific bit in the unsigned integer.
        /// </summary>
        /// <param name="Value">The unsigned integer value.</param>
        /// <param name="Index">The bit position (0-31).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        public static bool GetBit(this uint Value, int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        /// <summary>
        /// Sets or clears a specific bit in the unsigned integer.
        /// </summary>
        /// <param name="Value">The unsigned integer value.</param>
        /// <param name="Index">The bit position (0-31).</param>
        /// <param name="Bit">True to set the bit, false to clear it.</param>
        /// <returns>The modified unsigned integer value.</returns>
        public static uint SetBit(this uint Value, int Index, bool Bit)
        {
            if (Bit)
            {
                return Value | (uint)(1 << Index);
            }
            else
            {
                return (uint)(Value & ~(1 << Index));
            }
        }

        /// <summary>
        /// Checks if the unsigned integer value is even.
        /// </summary>
        /// <param name="Value">The unsigned integer value.</param>
        /// <returns>True if the value is even, otherwise false.</returns>
        public static bool IsEven(this uint Value)
        {
            return (Value & 1) == 0;
        }

        public static bool IsPrime(this uint Value)
        {
            if (Value <= 1) { return false; }
            if (Value == 2) { return true; }
            if (IsEven(Value)) { return false; }

            uint MaxValue = (uint)Math.Sqrt(Value);

            for (uint i = 3; i <= MaxValue; i += 2)
            {
                if (Value % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}