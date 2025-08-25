namespace Zion
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T>? Collection)
        {
            return Collection is null || Collection.Count == 0;
        }

        public static TOut[] ToArray<TIn, TOut>(this ICollection<TIn> Collection, Converter<TIn, TOut> Converter)
        {
            ArgumentNullException.ThrowIfNull(Collection);
            ArgumentNullException.ThrowIfNull(Converter);

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
            ArgumentNullException.ThrowIfNull(Converter);

            List<TOut> Result = new List<TOut>(Collection.Count + Math.Max(0, AdditionalCapacity));

            foreach (TIn Item in Collection)
            {
                Result.Add(Converter(Item));
            }

            return Result;
        }
    }
}