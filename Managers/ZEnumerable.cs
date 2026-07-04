namespace Zion
{
    public static class ZEnumerable
    {
        public static IEnumerable<int> Range(int Start, int End)
        {
            int Index = Start;

            while (Index < End)
            {
                yield return Index++;
            }
        }

        public static IEnumerable<int> Range(int Count)
        {
            if (int.IsNegative(Count)) { yield break; }

            int Index = 0;

            while (Index < Count)
            {
                yield return Index++;
            }
        }

        public static IEnumerable<int> Range<T>(T[] Array)
        {
            return Range(Array.Length);
        }

        public static IEnumerable<int> Range<T>(ICollection<T> Collection)
        {
            return Range(Collection.Count);
        }


        public static IEnumerable<int> For(int Start, int Count)
        {
            int End = Start + Count;
            return Range(Start, End);
        }


        public static IEnumerable<(T1, T2)> ToPair<T1, T2>(IEnumerable<T1> A, IEnumerable<T2> B)
        {
            using IEnumerator<T1> AEnumerator = Accessor.NotNull(A).GetEnumerator();
            using IEnumerator<T2> BEnumerator = Accessor.NotNull(B).GetEnumerator();

            while (true)
            {
                if (AEnumerator.MoveNext() && BEnumerator.MoveNext())
                {
                    yield return (AEnumerator.Current, BEnumerator.Current);
                }
                else
                {
                    break;
                }
            }
        }
    }
}