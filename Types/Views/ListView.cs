using System.Collections;

namespace Zion
{
    public sealed class ListView<T> : IEnumerable<T>
    {
        private readonly List<T> Source;

        public int Count => Source.Count;

        public ListView(List<T> Source)
        {
            this.Source = Source;
        }

        public T this[int Index] => Source[Index];

        public IEnumerator<T> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}