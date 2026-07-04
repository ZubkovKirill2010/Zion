namespace Zion
{
    public static class LongExtensions
    {
        extension(long Value)
        {
            /// <summary>
            /// Gets the state of a specific bit in the long integer.
            /// </summary>
            /// <param name="Value">The long integer value.</param>
            /// <param name="Index">The bit position (0-63).</param>
            /// <returns>True if the bit is set, otherwise false.</returns>
            public bool GetBit(int Index)
            {
                return (Value & (1L << Index)) != 0;
            }

            /// <summary>
            /// Sets or clears a specific bit in the long integer.
            /// </summary>
            /// <param name="Value">The long integer value.</param>
            /// <param name="Index">The bit position (0-63).</param>
            /// <param name="Bit">True to set the bit, false to clear it.</param>
            /// <returns>The modified long integer value.</returns>
            public long SetBit(int Index, bool Bit)
            {
                return Bit ? Value | (1L << Index) : Value & ~(1L << Index);
            }

            /// <summary>
            /// Checks if the long integer value is even.
            /// </summary>
            /// <param name="Value">The long integer value.</param>
            /// <returns>True if the value is even, otherwise false.</returns>
            public bool IsEven()
            {
                return (Value & 1L) == 0;
            }

            public bool IsPrime()
            {
                if (Value <= 1L) { return false; }
                if (Value == 2) { return true; }
                if (IsEven(Value)) { return false; }

                long MaxValue = (long)Math.Sqrt(Value);

                for (long i = 3; i <= MaxValue; i += 2)
                {
                    if (Value % i == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        extension(ulong Value)
        {
            /// <summary>
            /// Gets the state of a specific bit in the unsigned long integer.
            /// </summary>
            /// <param name="Value">The unsigned long integer value.</param>
            /// <param name="Index">The bit position (0-63).</param>
            /// <returns>True if the bit is set, otherwise false.</returns>
            public bool GetBit(int Index)
            {
                return (Value & 1UL << Index) != 0;
            }

            /// <summary>
            /// Sets or clears a specific bit in the unsigned long integer.
            /// </summary>
            /// <param name="Value">The unsigned long integer value.</param>
            /// <param name="Index">The bit position (0-63).</param>
            /// <param name="Bit">True to set the bit, false to clear it.</param>
            /// <returns>The modified unsigned long integer value.</returns>
            public ulong SetBit(int Index, bool Bit)
            {
                return Bit ? Value | 1UL << Index : Value & ~(1UL << Index);
            }

            /// <summary>
            /// Checks if the unsigned long integer value is even.
            /// </summary>
            /// <param name="Value">The unsigned long integer value.</param>
            /// <returns>True if the value is even, otherwise false.</returns>
            public bool IsEven()
            {
                return (Value & 1UL) == 0;
            }

            public bool IsPrime()
            {
                if (Value <= 1UL) { return false; }
                if (Value == 2) { return true; }
                if (IsEven(Value)) { return false; }

                ulong MaxValue = (ulong)Math.Sqrt(Value);

                for (ulong i = 3; i <= MaxValue; i += 2)
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
}