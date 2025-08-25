namespace Zion
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Determines whether the collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="Collection">The collection to check.</param>
        /// <returns>true if the collection is null or empty; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T>? Collection)
        {
            return Collection is null || Collection.Count == 0;
        }

        public static TOut[] ToArray<TIn, TOut>(this ICollection<TIn> Collection, Converter<TIn, TOut> Converter)
        {
            ArgumentNullException.ThrowIfNull(Collection);

            TOut[] Result = new TOut[Collection.Count];
            int Index = 0;

            foreach (TIn Item in Collection)
            {
                Result[Index++] = Converter(Item);
            }

            return Result;
        }

        public static List<TOut> ToList<TIn, TOut>(this ICollection<TIn> Collection, Converter<TIn, TOut> Converter, int AdditionalCapacity = 0)
        {
            ArgumentNullException.ThrowIfNull(Collection);

            List<TOut> Result = new List<TOut>(Collection.Count + Math.Max(0, AdditionalCapacity));

            foreach (TIn Item in Collection)
            {
                Result.Add(Converter(Item));
            }

            return Result;
        }
    }
}