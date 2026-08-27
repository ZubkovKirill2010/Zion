using System.Collections;

namespace Zion
{
    public sealed class ArenaSpan<T> : IDisposable, IComparable<ArenaSpan<T>>, IEnumerable<T>
    {
        private readonly Arena<T> Source;

        public readonly int Start;
        public readonly int Count;

        public bool IsDisposed { get; private set; }


        internal ArenaSpan(Arena<T> Source, int Start, int Size)
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

        public bool IsFrom(Arena<T> Arena)
        {
            return ReferenceEquals(Source, Arena);
        }


        public ArenaSpan<T> Expand(int Capacity)
        {
            ThrowIfDisposed();
            return Capacity > Count
                ? Source.Expand(this, Capacity)
                : this;
        }

        public void Move(int SourceIndex, int DestinationIndex, int Count)
        {
            if (Count <= 0 || SourceIndex == DestinationIndex)
            {
                return;
            }

            int MinIndex = Math.Min(SourceIndex, DestinationIndex);
            int MaxIndex = Math.Max(SourceIndex + Count, DestinationIndex + Count);

            ThrowIfWithout(MinIndex);
            ThrowIfWithout(MaxIndex);

            Arena<T> Arena = Source;

            int TotalWindowSize = MaxIndex - MinIndex;
            Span<T> TotalSpan   = Arena.GetSpan(Start + MinIndex, TotalWindowSize);

            int LocalSourceStart = SourceIndex - MinIndex;
            int LocalDestStart   = DestinationIndex - MinIndex;

            var SourceSlice      = TotalSpan.Slice(LocalSourceStart, Count);
            var DestinationSlice = TotalSpan.Slice(LocalDestStart, Count);

            SourceSlice.CopyTo(DestinationSlice);
        }



        public T[] ToArray(int Start, int Length)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Count);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Length, Count);

            return Source.ToArray(this.Start + Start, Length);
        }

        public ReadOnlySpan<T> AsSpan()
        {
            ThrowIfDisposed();
            return Source.AsSpan(Start, Count);
        }

        public ReadOnlySpan<T> AsSpan(int Start, int Length)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Count);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Length, Count);

            return Source.AsSpan(this.Start + Start, Length);
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
            if (!IsDisposed)
            {
                Source.Release(this);
                IsDisposed = true;
            }
        }


        public int CompareTo(ArenaSpan<T>? Other)
        {
            return Start.CompareTo(Other.NotNull().Start);
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
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(ArenaSpan<>));
            }
        }
    }
}