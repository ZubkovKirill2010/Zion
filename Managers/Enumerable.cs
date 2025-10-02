using System.Security.Cryptography;

namespace Zion
{
    public static class Enumerable
    {
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
    }
}