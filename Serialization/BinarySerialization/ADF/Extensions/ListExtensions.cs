using System.Runtime.InteropServices;

namespace Zion.Serialization.ADF
{
    internal static class ListExtensions
    {
        extension<T>(List<T> List)
        {
            public Span<T> AsSpan()
            {
                return CollectionsMarshal.AsSpan(List);
            }

            public Span<T> AsSpan(int Start, int Count)
            {
                var Span = CollectionsMarshal.AsSpan(List);

                if (Start < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Start), "Start is negative");
                }

                if (Count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Count), "Count is negative");
                }

                if (Start > Span.Length - Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(Start), "Start + Count exceeds list size");
                }

                return Span.Slice(Start, Count);
            }
        }
    }
}