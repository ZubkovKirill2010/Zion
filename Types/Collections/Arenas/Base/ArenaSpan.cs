using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Zion
{
    public struct ArenaSpan<T> : IDisposable
    {
        #region Data
        private readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.SupportsRecursion);

        internal readonly Arena<T> Source;

        public readonly int Start;
        public readonly int Count;

        private int Version;

        public bool IsDisposed { get; private set; }

        #endregion

        #region Constructors
        public ArenaSpan()
        {
            IsDisposed = true;
        }

        internal ArenaSpan(Arena<T> Source, int Start, int Size)
        {
            ArgumentNullException.ThrowIfNull(Source);
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Source.Capacity);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Size, Source.Capacity);

            this.Source = Source;
            this.Start = Start;
            this.Count = Size;
        }

        #endregion

        #region Indexers
        public T this[int Index]
        {
            get
            {
                ThrowIfDisposed();
                ThrowIfWithout(Index);
                Lock.EnterReadLock();
                try
                {
                    return Source[Start + Index];
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            set
            {
                ThrowIfDisposed();
                ThrowIfWithout(Index);
                Lock.EnterReadLock();
                Modify();
                try
                {
                    Source[Start + Index] = value;
                }
                finally
                {
                    Lock.ExitReadLock();
                }                
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

        #endregion

        #region Operators
        public static bool operator ==(ArenaSpan<T> A, ArenaSpan<T> B)
        {
            if (A.IsDisposed == B.IsDisposed)
            {
                return true;
            }
            else if (A.IsDisposed || B.IsDisposed)
            {
                return false;
            }

            return ReferenceEquals(A.Source, B.Source) && A.Start == B.Start && A.Count == B.Count;
        }

        public static bool operator !=(ArenaSpan<T> A, ArenaSpan<T> B)
        {
            return !(A == B);
        }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            return IsDisposed
                ? "[Disposed]"
                : $"[Start: {Start}; Count: {Count}]";
        }

        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is ArenaSpan<T> ArenaSpan && this == ArenaSpan;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Source, Start, Count);
        }

        #endregion

        #region PublicMethods
        public void UseSpan(Action<Span<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = Source.AsSpan(this);
                Action.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public void UseSpan(int Count, Action<Span<T>> Action)
        {
            UseSpan(0, Count, Action);
        }

        public void UseSpan(int Start, int Count, Action<Span<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = Source.AsSpan(this, Start, Count);
                Action.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public void UseSpan(int Start, int Count, Span<T> Other, Action<Span<T>, Span<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = Source.AsSpan(this, Start, Count);
                Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public void UseSpan(int Start, int Count, ReadOnlySpan<T> Other, Action<Span<T>, ReadOnlySpan<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = Source.AsSpan(this, Start, Count);
                Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        public void UseReadOnlySpan(Action<ReadOnlySpan<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> ReadOnlySpan = Source.AsSpan(this);
                Action.Invoke(ReadOnlySpan);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public void UseReadOnlySpan(int Count, Action<ReadOnlySpan<T>> Action)
        {
            UseReadOnlySpan(0, Count, Action);
        }

        public void UseReadOnlySpan(int Start, int Count, Action<ReadOnlySpan<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> ReadOnlySpan = Source.AsSpan(this, Start, Count);
                Action.Invoke(ReadOnlySpan);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public void UseReadOnlySpan(int Start, int Count, ReadOnlySpan<T> Other, Action<ReadOnlySpan<T>, ReadOnlySpan<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> ReadOnlySpan = Source.AsSpan(this, Start, Count);
                Action.Invoke(ReadOnlySpan, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        public I UseSpan<I>(Func<Span<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = Source.AsSpan(this);
                return Function.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public I UseSpan<I>(int Count, Func<Span<T>, I> Function)
        {
            return UseSpan(0, Count, Function);
        }

        public I UseSpan<I>(int Start, int Count, Func<Span<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = Source.AsSpan(this, Start, Count);
                return Function.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public I UseSpan<I>(int Start, int Count, Span<T> Other, Func<Span<T>, Span<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = Source.AsSpan(this, Start, Count);
                return Function.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public I UseSpan<I>(int Start, int Count, ReadOnlySpan<T> Other, Func<Span<T>, ReadOnlySpan<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = Source.AsSpan(this, Start, Count);
                return Function.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        public I UseReadOnlySpan<I>(Func<ReadOnlySpan<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> ReadOnlySpan = Source.AsSpan(this);
                return Function.Invoke(ReadOnlySpan);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public I UseReadOnlySpan<I>(int Count, Func<ReadOnlySpan<T>, I> Function)
        {
            return UseReadOnlySpan(0, Count, Function);
        }

        public I UseReadOnlySpan<I>(int Start, int Count, Func<ReadOnlySpan<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> ReadOnlySpan = Source.AsSpan(this, Start, Count);
                return Function.Invoke(ReadOnlySpan);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public I UseReadOnlySpan<I>(int Start, int Count, ReadOnlySpan<T> Other, Func<ReadOnlySpan<T>, ReadOnlySpan<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> ReadOnlySpan = Source.AsSpan(this, Start, Count);
                return Function.Invoke(ReadOnlySpan, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        public ArenaSpan<T> Expand(int Capacity)
        {
            ThrowIfDisposed();

            if (Capacity <= Count)
            {
                return this;
            }

            Lock.EnterWriteLock();
            Modify();
            try
            {
                return Source.Expand(this, Capacity);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public void Move(int SourceIndex, int DestinationIndex, int Count)
        {
            ThrowIfDisposed();
            if (Count <= 0 || SourceIndex == DestinationIndex) { return; }

            ThrowIfWithout(SourceIndex);
            ThrowIfWithout(SourceIndex + Count - 1);
            ThrowIfWithout(DestinationIndex);
            ThrowIfWithout(DestinationIndex + Count - 1);

            Lock.EnterWriteLock();
            Modify();
            try
            {
                Span<T> TotalSpan = Source.AsSpan(this);

                if (DestinationIndex > SourceIndex)
                {
                    if (SourceIndex + Count <= DestinationIndex)
                    {
                        TotalSpan.Slice(SourceIndex, Count)
                                 .CopyTo(TotalSpan.Slice(DestinationIndex, Count));
                    }
                    else
                    {
                        for (int i = Count - 1; i >= 0; i--)
                        {
                            TotalSpan[DestinationIndex + i] = TotalSpan[SourceIndex + i];
                        }
                    }
                }
                else
                {
                    if (DestinationIndex + Count <= SourceIndex)
                    {
                        TotalSpan.Slice(SourceIndex, Count)
                            .CopyTo(TotalSpan.Slice(DestinationIndex, Count));
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                        {
                            TotalSpan[DestinationIndex + i] = TotalSpan[SourceIndex + i];
                        }
                    }
                }
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public void CopyTo(Span<T> Destination)
        {
            Lock.EnterWriteLock();
            try
            {
                Source.AsSpan(this).CopyTo(Destination);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public void CopyTo(int Start, int Count, Span<T> Destination)
        {
            Lock.EnterWriteLock();
            try
            {
                Source.AsSpan(this, Start, Count).CopyTo(Destination);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }


        public void Modify()
        {
            Version++;
        }


        public bool IsWithin(int Index)
        {
            return Index >= 0 && Index < Count;
        }

        public bool IsWithout(int Index)
        {
            return Index < 0 || Index >= Count;
        }


        public T[] ToArray(int Start, int Length)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Count);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Length, Count);

            return Source.ToArray(this.Start + Start, Length);
        }


        public bool IsFrom(Arena<T> Arena)
        {
            return ReferenceEquals(Source, Arena);
        }

        #endregion

        #region IEnumerable
        public IEnumerator<T> GetEnumerator(IEnumerator<int> IndexEnumerator)
        {
            ThrowIfDisposed();
            return new Enumerator(this, IndexEnumerator);
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (IsDisposed) { return; }

            Lock.EnterWriteLock();
            Modify();
            try
            {
                Source.Release(this);
                IsDisposed = true;
            }
            finally
            {
                Lock.ExitWriteLock();
            }

            Lock.Dispose();
        }

        #endregion

        #region PrivateMethods
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

        #endregion

        #region Enumerator
        public struct Enumerator : IEnumerator<T>
        {
            private readonly IEnumerator<int> IndexEnumerator;
            private readonly ArenaSpan<T> Source;
            private readonly Memory<T> Memory;
            private readonly int Version;

            object? IEnumerator.Current => Current;

            public T Current { get; private set; }


            public Enumerator(ArenaSpan<T> Span, IEnumerator<int> IndexEnumerator)
            {
                this.IndexEnumerator = IndexEnumerator.NotNull();
                this.Source = Span;
                this.Memory = Span.Source.AsMemory(Span);
                this.Version = Span.Version;
            }


            public bool MoveNext()
            {
                CheckVersion();
                Source.ThrowIfDisposed();

                if (IndexEnumerator.MoveNext())
                {
                    int Index = IndexEnumerator.Current;
                    Current = Memory.Span[Index];
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                IndexEnumerator.Reset();
            }

            public void Dispose()
            {

            }


            private void CheckVersion()
            {
                if (Source.Version != Version)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
            }
        }

        #endregion
    }
}