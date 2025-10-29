using System.Collections;

namespace Zion
{
    public sealed class Slice<T> : IEnumerable<T>
    {
        private readonly IList<T> Items;
        private readonly int Start, Count;


        public Slice(IList<T> Items, int Start, int Count)
        {
            ArgumentNullException.ThrowIfNull(Items);
            if (Start.IsInRange(Items))
            {
                throw new ArgumentOutOfRangeException($"Start(={Start}) out of range Items.Count(={Items.Count})");
            }
            if (Count.IsInRange(Items))
            {
                throw new ArgumentOutOfRangeException($"End(={Start}) out of range Items.Count(={Items.Count})");
            }

            this.Items = Items;
            this.Start = Start;
            this.Count = Count;
        }

        public T this[int Index]
        {
            get
            {
                if (Index.IsInRange(Start, Count))
                {
                    return Items[Index];
                }
                throw new ArgumentOutOfRangeException($"Index out of range (Count={Count})");
            }
        }

        public bool Contains(T Item)
        {
            int Start = this.Start;
            for (int i = 0; i < Count; i++)
            {
                if (Items.Equals(Items[Start + i]))
                {
                    return true;
                }
            }
            return false;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (int i in Enumerable.Range(Start, Count))
            {
                yield return Items[i];
            }
        }
    }
}