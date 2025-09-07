namespace Zion
{
    public static class EnumerableExtensions
    {
        public static int Summarize(this IEnumerable<int> Enumerable) => Enumerable.Summarize(i => i);
        public static double Summarize(this IEnumerable<double> Enumerable) => Enumerable.Summarize(d => d);

        public static int Average(this IEnumerable<int> Enumerable) => Enumerable.Average(i => i);
        public static float Average(this IEnumerable<float> Enumerable) => Enumerable.Average(i => i);
        public static double Average(this IEnumerable<double> Enumerable) => Enumerable.Average(i => i);


        /// <summary>
        /// Summarizes elements by applying a length function to each item and summing the results.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Enumerable">The source collection.</param>
        /// <param name="GetLength">A function that extracts a length value from each element.</param>
        /// <returns>The total sum of lengths computed from all elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Enumerable"/> or <paramref name="GetLength"/> is null.</exception>
        public static int Summarize<T>(this IEnumerable<T> Enumerable, Func<T, int> GetLength)
        {
            int Count = 0;
            foreach (T Item in Enumerable)
            {
                Count += GetLength(Item);
            }
            return Count;
        }

        public static double Summarize<T>(this IEnumerable<T> Enumerable, Func<T, double> GetLength)
        {
            double Count = 0;
            foreach (T Item in Enumerable)
            {
                Count += GetLength(Item);
            }
            return Count;
        }


        public static int Average<T>(this IEnumerable<T> Enumerable, Func<T, int> ToInt)
        {
            int Result = 0;
            int Count = 0;

            foreach (T Item in Enumerable)
            {
                Result += ToInt(Item);
                Count++;
            }

            return Result / Count;
        }
        public static float Average<T>(this IEnumerable<T> Enumerable, Func<T, float> ToInt)
        {
            float Result = 0;
            int Count = 0;

            foreach (T Item in Enumerable)
            {
                Result += ToInt(Item);
                Count++;
            }

            return Result / Count;
        }

        public static double Average<T>(this IEnumerable<T> Enumerable, Func<T, double> ToInt)
        {
            double Result = 0;
            int Count = 0;

            foreach (T Item in Enumerable)
            {
                Result += ToInt(Item);
                Count++;
            }

            return Result / Count;
        }


        /// <summary>
        /// Determines whether all elements satisfy a condition.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Enumerable">The source collection.</param>
        /// <param name="Condition">A predicate to test each element.</param>
        /// <returns>
        /// <c>true</c> if every element passes the condition; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Enumerable"/> or <paramref name="Condition"/> is null.</exception>
        public static bool TrueForAll<T>(this IEnumerable<T> Enumerable, Predicate<T> Condition)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);
            ArgumentNullException.ThrowIfNull(Condition);

            foreach (T Item in Enumerable)
            {
                if (!Condition(Item))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether no elements satisfy a condition.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Enumerable">The source collection.</param>
        /// <param name="Condition">A predicate to test each element.</param>
        /// <returns>
        /// <c>true</c> if zero elements pass the condition; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Enumerable"/> or <paramref name="Condition"/> is null.</exception>
        public static bool FalseForAll<T>(this IEnumerable<T> Enumerable, Predicate<T> Condition)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);
            ArgumentNullException.ThrowIfNull(Condition);

            foreach (T Item in Enumerable)
            {
                if (Condition(Item))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Performs an action on each element of the collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Enumerable">The source collection.</param>
        /// <param name="Action">The action to execute on each element.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Enumerable"/> or <paramref name="Action"/> is null.</exception>
        /// <remarks>
        /// This method eagerly evaluates the sequence. For lazy evaluation, consider LINQ's <c>Select</c>.
        /// </remarks>
        public static void ForEach<T>(this IEnumerable<T> Enumerable, Action<T> Action)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);
            ArgumentNullException.ThrowIfNull(Action);

            foreach (T Item in Enumerable)
            {
                Action(Item);
            }
        }

        /// <summary>
        /// Repeats elements from the collection until a condition fails (lazy evaluation).
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Enumerable">The source collection.</param>
        /// <param name="Condition">A predicate to continue repetition. Stops when this returns <c>false</c>.</param>
        /// <returns>
        /// A sequence that repeats elements while the condition is satisfied.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Enumerable"/> or <paramref name="Condition"/> is null.</exception>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> Enumerable, Predicate<T> Condition)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);
            ArgumentNullException.ThrowIfNull(Condition);

            foreach (T Item in Enumerable)
            {
                yield return Item;
                if (!Condition(Item))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Repeats the entire collection a specified number of times (lazy evaluation).
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Enumerable">The source collection.</param>
        /// <param name="Count">The number of times to repeat the sequence. If 0, returns empty.</param>
        /// <returns>
        /// A sequence that repeats the original collection <paramref name="Count"/> times.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Enumerable"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="Count"/> is negative.</exception>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> Enumerable, int Count)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);
            if (Count < 0)
            {
                throw new ArgumentException($"Count(={Count}) < 0");
            }

            if (Count != 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    foreach (T Item in Enumerable)
                    {
                        yield return Item;
                    }
                }
            }
        }

        /// <summary>
        /// Generates an infinite sequence by repeating the source collection indefinitely (lazy evaluation).
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Enumerable">The source collection to repeat.</param>
        /// <returns>An infinite repeating sequence of the source elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Enumerable"/> is null.</exception>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> Enumerable)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);

            while (true)
            {
                foreach (T Item in Enumerable)
                {
                    yield return Item;
                }
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> Enumerable)
        {
            foreach (T Item in Enumerable)
            {
                return false;
            }
            return true;
        }


        public static string ToEnumerableString<T>(this IEnumerable<T> Enumerable)
        {
            return $"[{string.Join(", ", Enumerable)}]";
        }

        public static string ToEnumerableString<T>(this IEnumerable<T> Enumerable, Func<T, string> ToString)
        {
            return $"[{string.Join(", ", Enumerable.Select(ToString))}]";
        }
    }
}