using System.Collections;

namespace Zion
{
    public sealed class ArenaSpan<T> : IDisposable, IEnumerable<T>
    {
        private Arena<T> Source;

        private readonly int Start;
        public  readonly int Count;


        public ArenaSpan(Arena<T> Source, int Start, int Size)
        {
            ArgumentNullException.ThrowIfNull(Source);
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source.Capacity);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Size, Source.Capacity);

            this.Source = Source;
            this.Start = Start;
            this.Count = Size;
        }


        public T this[int Index]
        {
            get
            {
                ThrowIfDisposed();
                ThrowIfWithout(Index);
                return Source[Start + Index];
            }
            set
            {
                ThrowIfDisposed();
                ThrowIfWithout(Index);
                Source[Start + Index] = value;
            }
        }

        public T this[Index Index]
        {
            get
            {
                return this[Index.GetOffset(Count)];
            }
            set
            {
                this[Index.GetOffset(Count)] = value;
            }
        }


        public bool IsWithout(int Index)
        {
            ThrowIfDisposed();
            return Index < 0 || Index >= Count;
        }


        public ArenaSpan<T> Expand(int Capacity)
        {
            ThrowIfDisposed();
            return Capacity > Count
                ? Source.Expand(this, Capacity)
                : this;
        }


        public T[] ToArray(int Start, int Length)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Count);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Length, Count);

            return Source.ToArray(this.Start + Start, Length);
        }

        public Span<T> AsSpan()
        {
            ThrowIfDisposed();
            return Source.GetSpan(Start, Count);
        }

        public Span<T> AsSpan(int Start, int Length)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Count);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Length, Count);

            return Source.GetSpan(this.Start + Start, Length);
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            ThrowIfDisposed();
            return Source.GetEnumerator(Start, Count);
        }

        public IEnumerator<T> GetEnumerator(int Start, int Length)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Count);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Length, Count);

            return Source.GetEnumerator(this.Start, Length);
        }


        public void Dispose()
        {
            if (Source is null) { return; }

            Source.Release(this);
            Source = null;
        }


        private void ThrowIfWithout(int Index)
        {
            if (IsWithout(Index))
            {
                throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Count})");
            }
        }

        private void ThrowIfDisposed()
        {
            if (Source is null)
            {
                throw new ObjectDisposedException(nameof(ArenaSpan<>));
            }
        }
    }
}