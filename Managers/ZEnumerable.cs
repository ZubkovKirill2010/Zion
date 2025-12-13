namespace Zion
{
    public static class ZEnumerable
    {
        public static IEnumerable<int> Range(int Start, int End)
        {
            if (End <= Start) { yield break; }

            int Index = Start;

            while (Index != End)
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


        public static IEnumerable<(T1, T2)> ToPair<T1, T2>(IEnumerable<T1> A, IEnumerable<T2> B)
        {
            ArgumentNullException.ThrowIfNull(A);
            ArgumentNullException.ThrowIfNull(B);

            using IEnumerator<T1> AEnumerator = A.GetEnumerator();
            using IEnumerator<T2> BEnumerator = B.GetEnumerator();

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


        public static IEnumerable<T[]> GetAllPermutations<T>(params IList<T> Original)
        {
            ArgumentNullException.ThrowIfNull(Original);

            if (Original.Count == 0)
            {
                yield break;
            }

            if (Original.Count == 1)
            {
                yield return Original.ToArray();
                yield break;
            }

            foreach (T[] Permutation in GeneratePermutations(Original, 0))
            {
                yield return Permutation;
            }
        }


        private static IEnumerable<T[]> GeneratePermutations<T>(IList<T> Array, int Start)
        {
            if (Start >= Array.Count - 1)
            {
                yield return Array.ToArray();
                yield break;
            }

            for (int i = Start; i < Array.Count; i++)
            {
                if (!IsDuplicate(Array, Start, i))
                {
                    Swap(Array, Start, i);
                    foreach (T[] perm in GeneratePermutations(Array, Start + 1))
                    {
                        yield return perm;
                    }
                    Swap(Array, Start, i);
                }
            }
        }

        private static bool IsDuplicate<T>(IList<T> Array, int Start, int Current)
        {
            for (int i = Start; i < Current; i++)
            {
                if (EqualityComparer<T>.Default.Equals(Array[i], Array[Current]))
                {
                    return true;
                }
            }
            return false;
        }

        private static void Swap<T>(IList<T> Array, int i, int j)
        {
            (Array[i], Array[j]) = (Array[j], Array[i]);
        }
    }
}