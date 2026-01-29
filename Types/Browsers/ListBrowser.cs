using System.Collections;

namespace Zion
{
    public sealed class ListBrowser<T> : IEnumerable<T>
    {
        private readonly IList<T> List;

        public readonly int Count;

        public ListBrowser(IList<T> List)
        {
            ArgumentNullException.ThrowIfNull(List, "List");

            this.List = List;
            Count = List.Count;
        }

        public T this[int   Index] => List[Index];
        public T this[Index Index] => List[Index];

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }
    }
}