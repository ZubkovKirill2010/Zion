namespace Zion
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Inserts an element at the specified position in the array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="Array">The source array.</param>
        /// <param name="Index">The zero-based index at which to insert the value.</param>
        /// <param name="Value">The value to insert.</param>
        /// <returns>A new array with the inserted element.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the source array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of bounds.</exception>
        public static T[] Insert<T>(this T[] Array, int Index, T Value)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Array);

            T[] NewArray = new T[Array.Length + 1];
            System.Array.Copy(Array, 0, NewArray, 0, Index);
            NewArray[Index] = Value;
            System.Array.Copy(Array, Index, NewArray, Index + 1, Array.Length - Index);
            return NewArray;
        }


        public static (int, T)[] Index<T>(this T[] Array, int StartIndex = 0)
        {
            (int, T)[] Result = new (int, T)[Array.Length];

            for (int i = 0; i < Array.Length; i++)
            {
                int Index = i;
                Result[i] = (Index, Array[i]);
            }
            return Result;
        }

        public static IEnumerable<T> Range<T>(this T[] Array, int Start, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Start, nameof(Start));
            ArgumentOutOfRangeException.ThrowIfNegative(Count, nameof(Count));
            ArgumentOutOfRangeException.ThrowIf(Start + Count >= Array.Length, nameof(Count));

            foreach (int Index in ZEnumerable.For(Start, Count))
            {
                yield return Array[Index];
            }
        }
    }
}