namespace Zion
{
    public sealed class ArenaArray<T> : ArenaCollection<T>
    {
        public readonly new int Length;


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


        public new void UseSpan(Action<Span<T>> Action)
        {
            base.UseSpan(Action);
        }

        public new void UseSpan(int Count, Action<Span<T>> Action)
        {
            base.UseSpan(Count, Action);
        }

        public new void UseSpan(int Start, int Count, Action<Span<T>> Action)
        {
            base.UseSpan(Start, Count, Action);
        }

        public new void UseSpan(int Start, int Count, Span<T> Other, Action<Span<T>, Span<T>> Action)
        {
            base.UseSpan(Start, Count, Other, Action);
        }

        public new void UseSpan(int Start, int Count, ReadOnlySpan<T> Other, Action<Span<T>, ReadOnlySpan<T>> Action)
        {
            base.UseSpan(Start, Count, Other, Action);
        }


        public new void UseReadOnlySpan(Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Action);
        }

        public new void UseReadOnlySpan(int Count, Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Count, Action);
        }

        public new void UseReadOnlySpan(int Start, int Count, Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Start, Count, Action);
        }

        public new void UseReadOnlySpan(int Start, int Count, ReadOnlySpan<T> Other, Action<ReadOnlySpan<T>, ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Start, Count, Other, Action);
        }


        public new I UseSpan<I>(Func<Span<T>, I> Function)
        {
            return base.UseSpan(Function);
        }

        public new I UseSpan<I>(int Count, Func<Span<T>, I> Function)
        {
            return base.UseSpan(Count,  Function);
        }

        public new I UseSpan<I>(int Start, int Count, Span<T> Other, Func<Span<T>, Span<T>, I> Function)
        {
            return base.UseSpan(Start, Count, Other, Function);
        }

        public new I UseSpan<I>(int Start, int Count, ReadOnlySpan<T> Other, Func<Span<T>, ReadOnlySpan<T>, I> Function)
        {
            return base.UseSpan(Start, Count, Other, Function);
        }


        public new I UseReadOnlySpan<I>(Func<ReadOnlySpan<T>, I> Function)
        {
            return base.UseReadOnlySpan( Function);
        }

        public new I UseReadOnlySpan<I>(int Count, Func<ReadOnlySpan<T>, I> Function)
        {
            return base.UseReadOnlySpan(Count, Function);
        }

        public new I UseReadOnlySpan<I>(int Start, int Count, Func<ReadOnlySpan<T>, I> Function)
        {
            return base.UseReadOnlySpan(Start, Count, Function);
        }

        public new I UseReadOnlySpan<I>(int Start, int Count, ReadOnlySpan<T> Other, Func<ReadOnlySpan<T>, ReadOnlySpan<T>, I> Function)
        {
            return base.UseReadOnlySpan(Start, Count, Other, Function);
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