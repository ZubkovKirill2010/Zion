namespace Zion
{
    public static class CollectionExtensions
    {
        extension<TCollection, T>(TCollection Collection) where TCollection : ICollection<T>
        {
            /// <summary>
            /// Determines whether the collection is null or empty.
            /// </summary>
            /// <typeparam name="T">The type of elements in the collection.</typeparam>
            /// <param name="Collection">The collection to check.</param>
            /// <returns>true if the collection is null or empty; otherwise, false.</returns>
            public bool IsNullOrEmpty()
            {
                return Collection is null || Collection.Count == 0;
            }

            public List<T> ToList()
            {
                ArgumentNullException.ThrowIfNull(Collection);

                List<T> Result = new List<T>(Collection.Count);

                foreach (T Item in Collection)
                {
                    Result.Add(Item);
                }

                return Result;
            }


            public R[] ConvertAll<R>(Func<T, R> Converter)
            {
                ArgumentNullException.ThrowIfNull(Collection);
                ArgumentNullException.ThrowIfNull(Converter);

                R[] Result = new R[Collection.Count];
                int Index = 0;

                foreach (T Item in Collection)
                {
                    Result[Index++] = Converter(Item);
                }
                return Result;
            }


            public void AddIf(bool Condition, T Value)
            {
                if (Condition)
                {
                    Collection.Add(Value);
                }
            }

            public void AddIf(Func<bool> Condition, T Value)
            {
                if (Condition())
                {
                    Collection.Add(Value);
                }
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> Collection) where TKey : notnull
        {
            Dictionary<TKey, TValue> Result = new Dictionary<TKey, TValue>(Collection.NotNull().Count);

            foreach (KeyValuePair<TKey, TValue> Pair in Collection)
            {
                Result.Add(Pair.Key, Pair.Value);
            }

            return Result;
        }
    }
}