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
            return Data.Use(Span => Span.IndexOf(Item, Comparer));
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return Data.Use(Span => Span.Contains(Item, Comparer));
        }


        public void Clear()
        {
            Data.Use(static Span => Span.Clear());
        }


        public void Reverse()
        {
            Data.Use(static Span => Span.Reverse());
        }

        public void Sort()
        {
            Data.Use(static Span => Span.Sort());
        }


        public void UseSpan(Action<Span<T>> Action)
        {
            Data.Use(Action);
        }

        public T[] ToArray()
        {
            return Data.Use(static Span => Span.ToArray());
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            Data.CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(ArenaArray<T> Destination)
        {
            Destination.Data.Use(Data.CopyTo);
        }

        public void CopyTo(Span<T> Destination)
        {
            Data.CopyTo(Destination);
        }


        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException(); //TODO
        }
    }
}