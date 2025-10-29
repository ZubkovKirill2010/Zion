using System.Collections;

namespace Zion
{
    public sealed class ImmutableList<T> : IEnumerable<T>
    {
        private readonly IList<T> Items;
        public readonly int Count;

        public T this[int Index] => Items[Index];


        public ImmutableList(IList<T> Items)
        {
            this.Items = Items;
            Count = Items.Count;
        }


        public bool Contains(T Item)
        {
            return Items.Contains(Item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}