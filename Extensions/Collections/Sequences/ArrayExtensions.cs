namespace Zion
{
    public static class ArrayExtensions
    {
        extension<T>(T[] Array)
        {
            public (int, T)[] Index(int StartIndex = 0)
            {
                (int, T)[] Result = new (int, T)[Array.Length];

                for (int i = 0; i < Array.Length; i++)
                {
                    int Index = i;
                    Result[i] = (Index, Array[i]);
                }
                return Result;
            }

            public IEnumerable<T> Range(int Start, int Count)
            {
                ArgumentOutOfRangeException.ThrowIfNegative(Start, nameof(Start));
                ArgumentOutOfRangeException.ThrowIfNegative(Count, nameof(Count));
                ArgumentOutOfRangeException.ThrowIfBeyond(Start + Count, Array.Length);

                foreach (int Index in ZEnumerable.For(Start, Count))
                {
                    yield return Array[Index];
                }
            }

            public IEnumerator<T> Enumerate()
            {
                foreach (T Item in Array)
                {
                    yield return Item;
                }
            }
        }
    }
}