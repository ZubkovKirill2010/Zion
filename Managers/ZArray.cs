namespace Zion
{
    public static class ZArray
    {
        public static T[] Concat<T>(params T[][] Arrays)
        {
            if (Arrays.IsNullOrEmpty())
            {
                return System.Array.Empty<T>();
            }

            int Index = 0;
            int Count = Arrays.Summarize(Array => Array.Length);
            T[] Result = new T[Count];

            foreach (T[] Array in Arrays)
            {
                foreach (T Item in Array)
                {
                    Result[Index++] = Item;
                }
            }

            return Result;
        }

        public static void Repeat(Action Action, int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                Action();
            }
        }
        public static void Repeat(Action<int> Action, int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                int Index = i;
                Action(Index);
            }
        }

        public static T[] Clone<T>(T[] Source)
        {
            ArgumentNullException.ThrowIfNull(Source, "Source");

            T[] Result = new T[Source.Length];

            for (int i = 0; i < Source.Length; i++)
            {
                Result[i] = Source[i];
            }

            return Result;
        }

        public static TOut[] TryConvertAll<TIn, TOut>(this ICollection<TIn> Input, SafeConverter<TIn, TOut> Converter)
        {
            ArgumentNullException.ThrowIfNull(Input, nameof(Input));
            ArgumentNullException.ThrowIfNull(Converter, nameof(Converter));

            int Count = Input.Count;
            int ItemCount = 0;

            TOut[] Result = new TOut[Input.Count];

            foreach (TIn In in Input)
            {
                if (Converter(In, out TOut Item))
                {
                    Result[ItemCount++] = Item;
                }
            }

            return Result.Length == Count ? Result : Result[..ItemCount];
        }
    }
}