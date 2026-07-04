namespace Zion
{
    public static class ArrayExtensions
    {
        public static (int, T)[] Index<T>(this T[] Array, int StartIndex = 0)
        {
            (int, T)[] Result = new (int, T)[Array.Length];

            for (int i = 0; i < Array.Length; i++)
            {
                int Index = i;
                Result[i] = (Index, Array[i]);
            }
            return Result;
        }

        public static IEnumerable<T> Range<T>(this T[] Array, int Start, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Start, nameof(Start));
            ArgumentOutOfRangeException.ThrowIfNegative(Count, nameof(Count));
            ArgumentOutOfRangeException.ThrowIf(Start + Count >= Array.Length, nameof(Count));

            foreach (int Index in ZEnumerable.For(Start, Count))
            {
                yield return Array[Index];
            }
        }
    }
}