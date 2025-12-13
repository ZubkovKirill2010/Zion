namespace Zion
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Adds an element to the end of the array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="Array">The source array.</param>
        /// <param name="Value">The value to add.</param>
        /// <returns>A new array with the added element.</returns>
        /// <exception cref="NullReferenceException">Thrown when the source array is null.</exception>
        public static T[] Add<T>(this T[] Array, T Value)
        {
            ArgumentNullException.ThrowIfNull(Array);

            T[] NewArray = new T[Array.Length + 1];
            for (int i = 0; i < Array.Length; i++)
            {
                NewArray[i] = Array[i];
            }
            NewArray[Array.Length] = Value;
            return NewArray;
        }

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

        /// <summary>
        /// Removes the element at the specified position from the array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="Array">The source array.</param>
        /// <param name="Index">The zero-based index of the element to remove.</param>
        /// <returns>A new array without the specified element.</returns>
        /// <exception cref="NullReferenceException">Thrown when the source array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of bounds.</exception>
        public static T[] RemoveAt<T>(this T[] Array, int Index)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Array);

            T[] NewArray = new T[Array.Length - 1];
            System.Array.Copy(Array, NewArray, Index);
            if (Index < Array.Length - 1)
            {
                System.Array.Copy(Array, Index + 1, NewArray, Index, Array.Length - Index - 1);
            }
            return NewArray;
        }

        /// <summary>
        /// Creates a shallow copy of the array (for value types only).
        /// </summary>
        /// <typeparam name="T">The type of elements in the array (must be a value type).</typeparam>
        /// <param name="Array">The source array.</param>
        /// <returns>A new array with copied elements.</returns>
        public static T[] Clone<T>(this T[] Array) where T : struct
        {
            T[] Result = new T[Array.Length];
            for (int i = 0; i < Array.Length; i++)
            {
                Result[i] = Array[i];
            }
            return Result;
        }

        public static T[] Where<T>(this ICollection<T> Collection, Predicate<T> Condition)
        {
            List<T> Result = new List<T>(Collection.Count);

            foreach (T? Item in Collection)
            {
                if (Condition(Item))
                {
                    Result.Add(Item);
                }
            }

            return Result.ToArray(); ;
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
    }
}