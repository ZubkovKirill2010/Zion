namespace Zion
{
    public static class ZArray
    {
        public static T[] Concat<T>(params T[][] Arrays)
        {
            if (Arrays.IsNullOrEmpty()) { return Array.Empty<T>(); }

            int TotalLength = 0;
            foreach (T[] Array in Arrays)
            {
                TotalLength += Array.Length;
            }

            T[] Result = new T[TotalLength];
            int Offset = 0;

            foreach (T[] Array in Arrays)
            {
                System.Array.Copy(Array, 0, Result, Offset, Array.Length);
                Offset += Array.Length;
            }

            return Result;
        }

        public static T[] Concat<T>(T[] Array, T Value)
        {
            ArgumentNullException.ThrowIfNull(Array);

            T[] Result = new T[Array.Length + 1];
            System.Array.Copy(Array, 0, Result, 0, Array.Length);
            Result[Array.Length] = Value;

            return Result;
        }

        public static T[] Insert<T>(T[] Array, int Index, T Value)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Array);

            T[] Result = new T[Array.Length + 1];
            Result[Index] = Value;

            System.Array.Copy(Array, 0, Result, 0, Index);
            System.Array.Copy(Array, Index, Result, Index + 1, Array.Length - Index);

            return Result;
        }

        public static T[] RemoveAt<T>(T[] Array, int Index)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Array);

            T[] Result = new T[Array.Length - 1];

            System.Array.Copy(Array, 0, Result, 0, Index);
            System.Array.Copy(Array, Index + 1, Result, Index, Array.Length - Index - 1);

            return Result;
        }

        public static T[] Clone<T>(T[] Array)
        {
            ArgumentNullException.ThrowIfNull(Array);

            T[] Result = new T[Array.Length];
            System.Array.Copy(Array, 0, Result, 0, Array.Length);
            return Result;
        }

        public static T[] FromCollection<T>(ICollection<T> Collection)
        {
            ArgumentNullException.ThrowIfNull(Collection);

            T[] Result = new T[Collection.Count];
            Collection.CopyTo(Result, 0);
            return Result;
        }

        public static TOut[] ConvertAll<TIn, TOut>(this ICollection<TIn> Source, SafeConverter<TIn, TOut> Converter)
        {
            ArgumentNullException.ThrowIfNull(Source);
            ArgumentNullException.ThrowIfNull(Converter);

            int Count = Source.Count;
            TOut[] Result = new TOut[Count];
            int ItemCount = 0;

            foreach (TIn Item in Source)
            {
                if (Converter(Item, out TOut Converted))
                {
                    Result[ItemCount++] = Converted;
                }
            }

            return ItemCount == Count ? Result : Result[..ItemCount];
        }

        public static bool TryConvertAll<TIn, TOut>(this ICollection<TIn> Source, SafeConverter<TIn, TOut> Converter, out TOut[] Converted)
        {
            ArgumentNullException.ThrowIfNull(Source);
            ArgumentNullException.ThrowIfNull(Converter);

            int Count = Source.Count;
            Converted = new TOut[Count];
            int ItemCount = 0;

            foreach (TIn Item in Source)
            {
                if (Converter(Item, out TOut ConvertedItem))
                {
                    Converted[ItemCount++] = ConvertedItem;
                }
                else
                {
                    Converted = Array.Empty<TOut>();
                    return false;
                }
            }

            return true;
        }
    }
}