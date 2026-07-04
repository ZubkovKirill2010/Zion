namespace Zion
{
    public static class EnumerableExtensions
    {
        extension<T>(IEnumerable<T> Enumerable)
        {
            public void ForEach(Action<T> Action)
            {
                ArgumentNullException.ThrowIfNull(Enumerable);
                ArgumentNullException.ThrowIfNull(Action);

                foreach (T Item in Enumerable)
                {
                    Action(Item);
                }
            }

            public T[] ToArray(int Count)
            {
                T[] Array = new T[Count];
                int Index = 0;

                foreach (T Item in Enumerable)
                {
                    if (Index >= Count)
                    {
                        throw new ArgumentOutOfRangeException($"Count of Enumerable >= Count(={Count})");
                    }

                    Array[Index++] = Item;
                }

                return Array;
            }


            public bool IsNullOrEmpty()
            {
                if (Enumerable is null)
                {
                    return true;
                }

                using IEnumerator<T> Enumerator = Enumerable.GetEnumerator();
                return !Enumerator.MoveNext();
            }

            public bool None(Func<T, bool> Condition)
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

            public bool HasAtLeast(int MinCount)
            {
                ArgumentNullException.ThrowIfNull(Enumerable);
                ArgumentOutOfRangeException.ThrowIfNegative(MinCount);

                int Count = 0;
                foreach (T Item in Enumerable)
                {
                    if (++Count >= MinCount)
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool Begins(IEnumerable<T> Target, Func<T, T, bool>? Equals)
            {
                Equals ??= (A, B) => A?.Equals(B) ?? false;
                return ZEnumerable.ToPair(Enumerable.NotNull(), Target.NotNull()).All(Pair => Equals(Pair.Item1, Pair.Item2));
            }


            public IEnumerable<T> WhereNotNull()
            {
                ArgumentNullException.ThrowIfNull(Enumerable);

                foreach (T? Item in Enumerable)
                {
                    if (Item is not null)
                    {
                        yield return Item;
                    }
                }
            }

            public IEnumerable<T> Limit(int Count)
            {
                ArgumentNullException.ThrowIfNull(Enumerable);
                ArgumentOutOfRangeException.ThrowIfNegative(Count);

                if (Count == 0)
                {
                    yield break;
                }

                int Index = 0;

                foreach (T Value in Enumerable)
                {
                    if (Index == Count)
                    {
                        yield break;
                    }

                    yield return Value;
                    Index++;
                }
            }
        }

        public static int Average(this IEnumerable<int> Enumerable)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);

            int Sum = 0;
            int Count = 0;

            foreach (int Item in Enumerable)
            {
                Sum += Item;
                Count++;
            }

            return Count == 0 ? 0 : Sum / Count;
        }

        public static float Average(this IEnumerable<float> Enumerable)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);

            float Sum = 0;
            int Count = 0;

            foreach (float Item in Enumerable)
            {
                Sum += Item;
                Count++;
            }

            return Count == 0 ? 0 : Sum / Count;
        }

        public static double Average(this IEnumerable<double> Enumerable)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);

            double Sum = 0;
            int Count = 0;

            foreach (double Item in Enumerable)
            {
                Sum += Item;
                Count++;
            }

            return Count == 0 ? 0 : Sum / Count;
        }

        public static decimal Average(this IEnumerable<decimal> Enumerable)
        {
            ArgumentNullException.ThrowIfNull(Enumerable);

            decimal Sum = 0;
            int Count = 0;

            foreach (decimal Item in Enumerable)
            {
                Sum += Item;
                Count++;
            }

            return Count == 0 ? 0 : Sum / Count;
        }
    }
}