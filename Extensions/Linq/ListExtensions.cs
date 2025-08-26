using System.Runtime.CompilerServices;

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
    }
}