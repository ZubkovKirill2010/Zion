namespace Zion
{
    public sealed class ArenaArray<T> : ArenaCollection<T>
    {
        public readonly int Length;


        public ArenaArray(ArenaSpan<T> Data) : base(Data)
        {
            Length = Data.Count;
        }


        public T this[int Index]
        {
            get => Data[Index];
            set
            {
                var Span = Data;
                Span[Index] = value;
            }
        }

        public T this[Index Index]
        {
            get => this[Index.GetOffset(Length)];
            set => this[Index.GetOffset(Length)] = value;
        }


        public T First()
        {
            return Data[0];
        }

        public T Last()
        {
            return Data[Length - 1];
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return Data.UseSpan(Span => Span.IndexOf(Item, Comparer));
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return Data.UseSpan(Span => Span.Contains(Item, Comparer));
        }


        public void Clear()
        {
            Data.UseSpan(static Span => Span.Clear());
        }


        public void Reverse()
        {
            Data.UseSpan(static Span => Span.Reverse());
        }

        public void Sort()
        {
            Data.UseSpan(static Span => Span.Sort());
        }


        public void UseSpan(Action<Span<T>> Action)
        {
            Data.UseSpan(Action);
        }

        public T[] ToArray()
        {
            return Data.UseSpan(static Span => Span.ToArray());
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            Data.CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(ArenaArray<T> Destination)
        {
            Destination.Data.UseSpan(Data.CopyTo);
        }

        public void CopyTo(Span<T> Destination)
        {
            Data.CopyTo(Destination);
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