namespace Zion
{
    public static class EnumerableExtensions
    {
        public static int Summarize<T>(this IEnumerable<T> Array, Func<T, int> GetLength)
        {
            int Count = 0;
            foreach (T Item in Array)
            {
                Count += GetLength(Item);
            }
            return Count;
        }

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

        public static void ForEach<T>(this IEnumerable<T> Enumerable, Action<T> Action)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);
            ArgumentNullException.ThrowIfNull(Action);

            foreach (T Item in Enumerable)
            {
                Action(Item);
            }
        }

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
    }
}