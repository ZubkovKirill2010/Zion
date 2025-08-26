namespace Zion
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty<T>(this IList<T>? List)
        {
            return List is null || List.Count == 0;
        }

        public static void Add<T>(this IList<T> List, T Value, Predicate<T> Condition)
        {
            if (Condition(Value))
            {
                List.Add(Value);
            }
        }


        public static R[] ConvertAll<T, R>(this IList<T> List, Converter<T, R> Converter)
        {
            R[] Result = new R[List.Count];
            for (int i = 0; i < Result.Length; i++)
            {
                Result[i] = Converter(List[i]);
            }
            return Result;
        }

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