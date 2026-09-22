namespace Zion
{
    public sealed class ArenaArray<T> : ArenaCollection<T>
    {
        public readonly int Length;


        public ArenaArray(ArenaSpan<T> Data) : base(Data)
        {
            Length = base.Length;
        }


        public new T this[int Index]
        {
            get => base[Index];
            set => base[Index] = value;
        }

        public new T this[Index Index]
        {
            get => this[Index];
            set => this[Index] = value;
        }


        public T First()
        {
            return this[0];
        }

        public T Last()
        {
            return this[Length - 1];
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Span => Span.IndexOf(Item, Comparer));
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Span => Span.Contains(Item, Comparer));
        }


        public void Clear()
        {
            UseSpan(static Span => Span.Clear());
        }


        public void Reverse()
        {
            UseSpan(static Span => Span.Reverse());
        }

        public void Sort()
        {
            UseSpan(static Span => Span.Sort());
        }


        public new void Expand(int Additional)
        {
            Expand(Additional);
        }


        public T[] ToArray()
        {
            return UseReadOnlySpan(static Span => Span.ToArray());
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(ArenaArray<T> Destination)
        {
            Destination.UseSpan(CopyTo);
        }

        public new void CopyTo(Span<T> Destination)
        {
            base.CopyTo(Destination);
        }


        protected override IEnumerator<int> GetIndexEnumerator()
        {
            int Count = Length;
            for (int i = 0; i < Count; i++)
            {
                yield return i;
            }
        }
    }
}