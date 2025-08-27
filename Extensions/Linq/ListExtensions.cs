namespace Zion
{
    public static class ListExtensions
    {
        /// <summary>
        /// Checks if the list is either null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="List">The list to check (can be null).</param>
        /// <returns>
        /// <c>true</c> if the list is null or has no elements; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IList<T>? List)
        {
            return List is null || List.Count == 0;
        }

        /// <summary>
        /// Adds an element to the list only if the specified condition is met.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="List">The list to which the element may be added.</param>
        /// <param name="Value">The value to add.</param>
        /// <param name="Condition">A predicate that determines whether the value should be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="List"/> or <paramref name="Condition"/> is null.</exception>
        public static void Add<T>(this IList<T> List, T Value, Predicate<T> Condition)
        {
            if (Condition(Value))
            {
                List.Add(Value);
            }
        }

        /// <summary>
        /// Converts all elements of the list to another type using the specified converter.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <typeparam name="R">The type of elements in the resulting array.</typeparam>
        /// <param name="List">The source list.</param>
        /// <param name="Converter">A delegate that converts each element.</param>
        /// <returns>An array of the converted elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="List"/> or <paramref name="Converter"/> is null.</exception>
        public static R[] ConvertAll<T, R>(this IList<T> List, Converter<T, R> Converter)
        {
            R[] Result = new R[List.Count];
            for (int i = 0; i < Result.Length; i++)
            {
                Result[i] = Converter(List[i]);
            }
            return Result;
        }

        /// <summary>
        /// Creates a new array with elements in reverse order compared to the original list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="List">The source list.</param>
        /// <returns>A new array containing elements in reversed order.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="List"/> is null.</exception>
        public static T[] ToReversedArray<T>(this IList<T> List)
        {
            T[] Result = new T[List.Count];

            int Start = 0;
            int End = List.Count - 1;

            while (End >= 0)
            {
                Result[Start++] = List[End--];
            }
            return Result;
        }

        /// <summary>
        /// Removes elements at the specified indexes from the list efficiently (preserves order).
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="List">The list from which elements will be removed.</param>
        /// <param name="Indexes">The indexes of elements to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any index is out of bounds.</exception>
        /// <remarks>
        /// This method avoids shifting elements multiple times by performing a single pass.
        /// Duplicate indexes are automatically filtered out.
        /// </remarks>
        public static void RemoveAt<T>(this List<T> List, params int[] Indexes)
        {
            if (Indexes.Length == 0 || List.Count == 0)
            {
                return;
            }

            Indexes = Indexes.Distinct().OrderBy(i => i).ToArray();

            if (Indexes[0] < 0 || Indexes[^1] >= List.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(Indexes), "One or more indexes are out of range");
            }

            int WritePosition = 0;
            int ReadPosition = 0;
            int DeletedIndex = 0;

            while (ReadPosition < List.Count && DeletedIndex < Indexes.Length)
            {
                if (ReadPosition == Indexes[DeletedIndex])
                {
                    ReadPosition++;
                    DeletedIndex++;
                }
                else
                {
                    if (WritePosition != ReadPosition)
                    {
                        List[WritePosition] = List[ReadPosition];
                    }
                    WritePosition++;
                    ReadPosition++;
                }
            }

            while (ReadPosition < List.Count)
            {
                List[WritePosition++] = List[ReadPosition++];
            }

            List.RemoveRange(WritePosition, List.Count - WritePosition);
        }

        /// <summary>
        /// Generates a sequence that goes forward through the collection and then backward.
        /// For example: [1, 2, 3] -> 1, 2, 3, 2, 1
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="List">The source collection</param>
        /// <returns>An oscillating sequence of elements</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input collection is null</exception>
        public static IEnumerable<T> Oscillate<T>(this IList<T> List)
        {
            ArgumentNullException.ThrowIfNull(List);

            if (List.Count == 0)
            {
                yield break;
            }

            int End = List.Count - 1;

            for (int i = 0; i < End; i++)
            {
                yield return List[i];
            }
            for (int j = End; j >= 0; j--)
            {
                yield return List[j];
            }
        }

        /// <summary>
        /// Searches for the last element that matches the condition and returns its index.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="List">The list to search</param>
        /// <param name="Condition">The condition to test each element</param>
        /// <returns>The index of the last matching element, or -1 if not found</returns>
        /// <exception cref="ArgumentNullException">Thrown when list or condition is null</exception>
        public static int EndIndexOf<T>(this IList<T> List, Predicate<T> Condition)
        {
            ArgumentNullException.ThrowIfNull(List);
            ArgumentNullException.ThrowIfNull(Condition);

            for (int i = List.Count - 1; i >= 0; i--)
            {
                if (Condition(List[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int IndexOf<T>(this IList<T> List, Predicate<T> Condition)
        {
            ArgumentNullException.ThrowIfNull(List);
            ArgumentNullException.ThrowIfNull(Condition);

            for (int i = 0; i < List.Count; i++)
            {
                if (Condition(List[i]))
                {
                    return i;
                }
            }

            return -1;
        }


        public static int Summarize<T>(this IList<T> List, Func<T, int> GetLength, int End)
        {
            if (!End.IsClamp(0, List.Count - 1))
            {
                throw new ArgumentOutOfRangeException(nameof(End) ,$"End(={End}) < 0 or >= List.Count(={List.Count})");
            }

            int Count = 0;
            for (int i = 0; i < End; i++)
            {
                Count += GetLength(List[i]);
            }
            return Count;
        }
    }
}