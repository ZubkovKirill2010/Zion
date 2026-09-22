using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Zion
{
    public abstract class ArenaCollection<T> : IDisposable, IEnumerable<T>
    {
        #region Data
        private readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.SupportsRecursion);

        internal Arena<T> Source
        {
            get => field is not null
                ? field
                : throw new ObjectDisposedException(GetType().Name);
            private set;
        }

        internal int Start;
        internal int Length;

        private int Version;

        #endregion

        #region Properties
        public bool IsDisposed => Source is null;

        #endregion

        #region Constructors
        public ArenaCollection(ArenaSpan<T> Span)
        {
            Source = Span.Source;
            Start  = Span.Start;
            Length = Span.Length;
        }

        #endregion

        #region Indexers
        public T this[int Index]
        {
            get
            {
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
                return this[Index.GetOffset(Length)];
            }
            set
            {
                this[Index.GetOffset(Length)] = value;
            }
        }

        #endregion

        #region Operators
        public static bool operator ==(ArenaCollection<T> A, ArenaCollection<T> B)
        {
            if (A.IsDisposed == B.IsDisposed)
            {
                return true;
            }
            else if (A.IsDisposed || B.IsDisposed)
            {
                return false;
            }

            return ReferenceEquals(A.Source, B.Source) && A.Start == B.Start && A.Length == B.Length;
        }

        public static bool operator !=(ArenaCollection<T> A, ArenaCollection<T> B)
        {
            return !(A == B);
        }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            return IsDisposed
                ? "[Disposed]"
                : StringFormatter.ToString(this);
        }

        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is ArenaCollection<T> Collection && this == Collection;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Source, Start, Length);
        }

        #endregion

        #region AbstractMethods
        protected abstract IEnumerator<int> GetIndexEnumerator();
        #endregion

        #region PublicMethods

        public bool IsFrom(Arena<T> Arena)
        {
            return IsFrom(Arena);
        }

        #endregion

        #region ProtectedMethods
        protected void UseSpan(Action<Span<T>> Action)
        {
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

        protected void UseSpan(int Count, Action<Span<T>> Action)
        {
            UseSpan(0, Count, Action);
        }

        protected void UseSpan(int Start, int Count, Action<Span<T>> Action)
        {
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

        protected void UseSpan(int Start, int Count, Span<T> Other, Action<Span<T>, Span<T>> Action)
        {
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

        protected void UseSpan(int Start, int Count, ReadOnlySpan<T> Other, Action<Span<T>, ReadOnlySpan<T>> Action)
        {
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


        protected void UseReadOnlySpan(Action<ReadOnlySpan<T>> Action)
        {
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

        protected void UseReadOnlySpan(int Count, Action<ReadOnlySpan<T>> Action)
        {
            UseReadOnlySpan(0, Count, Action);
        }

        protected void UseReadOnlySpan(int Start, int Count, Action<ReadOnlySpan<T>> Action)
        {
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

        protected void UseReadOnlySpan(int Start, int Count, ReadOnlySpan<T> Other, Action<ReadOnlySpan<T>, ReadOnlySpan<T>> Action)
        {
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


        protected I UseSpan<I>(Func<Span<T>, I> Function)
        {
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

        protected I UseSpan<I>(int Count, Func<Span<T>, I> Function)
        {
            return UseSpan(0, Count, Function);
        }

        protected I UseSpan<I>(int Start, int Count, Func<Span<T>, I> Function)
        {
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

        protected I UseSpan<I>(int Start, int Count, Span<T> Other, Func<Span<T>, Span<T>, I> Function)
        {
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

        protected I UseSpan<I>(int Start, int Count, ReadOnlySpan<T> Other, Func<Span<T>, ReadOnlySpan<T>, I> Function)
        {
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


        protected I UseReadOnlySpan<I>(Func<ReadOnlySpan<T>, I> Function)
        {
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

        protected I UseReadOnlySpan<I>(int Count, Func<ReadOnlySpan<T>, I> Function)
        {
            return UseReadOnlySpan(0, Count, Function);
        }

        protected I UseReadOnlySpan<I>(int Start, int Count, Func<ReadOnlySpan<T>, I> Function)
        {
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

        protected I UseReadOnlySpan<I>(int Start, int Count, ReadOnlySpan<T> Other, Func<ReadOnlySpan<T>, ReadOnlySpan<T>, I> Function)
        {
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


        protected void Expand(int Additional)
        {
            EnsureCapacity(Length + Additional);
        }

        protected void EnsureCapacity(int Capacity)
        {
            if (Capacity <= Length)
            {
                return;
            }

            Lock.EnterWriteLock();
            Modify();
            try
            {
                var Area = Source.Expand(this, Capacity);
                Start = Area.Start;
                Length = Area.Count;
            }
            finally
            {
                Lock.ExitWriteLock();
            }

        }

        protected void Move(int SourceIndex, int DestinationIndex, int Count)
        {
            if (Count <= 0 || SourceIndex == DestinationIndex)
            {
                return;
            }

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

        protected void CopyTo(Span<T> Destination)
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

        protected void CopyTo(int Start, int Count, Span<T> Destination)
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


        protected void Modify()
        {
            Version++;
        }


        protected bool IsWithin(int Index)
        {
            return Index >= 0 && Index < Length;
        }

        protected bool IsWithout(int Index)
        {
            return Index < 0 || Index >= Length;
        }


        protected T[] ToArray(int Start, int Length)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, this.Length);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Length, this.Length);

            return Source.ToArray(this.Start + Start, Length);
        }

        #endregion

        #region PrivateMethods
        private void ThrowIfWithout(int Index)
        {
            if (IsWithout(Index))
            {
                throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Length})");
            }
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
                ResetToZero();
            }
            finally
            {
                Lock.ExitWriteLock();
            }

            Lock.Dispose();
        }

        public void ResetToZero()
        {
            Source = null!;
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this, GetIndexEnumerator());
        }

        #endregion

        #region Enumerator
        public struct Enumerator : IEnumerator<T>
        {
            private readonly IEnumerator<int> IndexEnumerator;
            private readonly ArenaCollection<T> Collection;
            private readonly Memory<T> Memory;
            private readonly int Version;

            object? IEnumerator.Current => Current;

            public T Current { get; private set; }


            public Enumerator(ArenaCollection<T> Collection, IEnumerator<int> IndexEnumerator)
            {
                this.Collection = Collection;
                this.IndexEnumerator = IndexEnumerator.NotNull();
                this.Memory = Collection.Source.AsMemory(Collection);
                this.Version = Collection.Version;
            }


            public bool MoveNext()
            {
                CheckVersion();

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

            public void Dispose() { }


            private void CheckVersion()
            {
                if (Collection.Version != Version)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
            }
        }

        #endregion
    }
}