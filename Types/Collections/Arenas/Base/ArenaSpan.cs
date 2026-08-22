using System.Collections;

namespace Zion
{
    public sealed class ArenaSpan<T> : IDisposable, IEnumerable<T>
    {
        private Arena<T> Source;
        private int Start;

        public int Count { get; private set; }


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

        public void ThrowIfWithout(int Index)
        {
            if (IsWithout(Index))
            {
                throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Count})");
            }
        }

        public void ThrowIfDisposed()
        {
            if (Source is null)
            {
                throw new ObjectDisposedException(nameof(ArenaSpan<>));
            }
        }



        public void Dispose()
        {
            Source.Release(this);
            Source = null;
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            ThrowIfDisposed();
            return Source.GetEnumerator(Start, Count);
        }
    }
}