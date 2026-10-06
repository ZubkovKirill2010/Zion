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

        protected abstract int GetSpanLimit();

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
                Span<T> Span = AsSpan();
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
                Span<T> Span = AsSpan(Start, Count);
                Action.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected void UseSpan<I>(I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = AsSpan();
                Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected void UseSpan<I>(int Count, I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            UseSpan(0, Count, Other, Action);
        }

        protected void UseSpan<I>(int Start, int Count, I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = AsSpan(Start, Count);
                Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected R UseSpan<R>(Func<Span<T>, R> Function)
        {
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = AsSpan();
                return Function.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected R UseSpan<R>(int Count, Func<Span<T>, R> Function)
        {
            return UseSpan(0, Count, Function);
        }

        protected R UseSpan<R>(int Start, int Count, Func<Span<T>, R> Function)
        {
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = AsSpan(Start, Count);
                return Function.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected R UseSpan<I, R>(I Other, Func<Span<T>, I, R> Action) where I : allows ref struct where R : allows ref struct
        {
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = AsSpan();
                return Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected R UseSpan<I, R>(int Count, I Other, Func<Span<T>, I, R> Action) where I : allows ref struct where R : allows ref struct
        {
            return UseSpan(0, Count, Other, Action);
        }

        protected R UseSpan<I, R>(int Start, int Count, I Other, Func<Span<T>, I, R> Action) where I : allows ref struct where R : allows ref struct
        {
            Lock.EnterReadLock();
            Modify();
            try
            {
                Span<T> Span = AsSpan(Start, Count);
                return Action.Invoke(Span, Other);
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
                ReadOnlySpan<T> ReadOnlySpan = AsSpan();
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
                ReadOnlySpan<T> ReadOnlySpan = AsSpan(Start, Count);
                Action.Invoke(ReadOnlySpan);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected void UseReadOnlySpan<I>(I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> Span = AsSpan();
                Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected void UseReadOnlySpan<I>(int Count, I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            UseReadOnlySpan(0, Count, Other, Action);
        }

        protected void UseReadOnlySpan<I>(int Start, int Count, I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> Span = AsSpan(Start, Count);
                Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected R UseReadOnlySpan<R>(Func<ReadOnlySpan<T>, R> Function)
        {
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> Span = AsSpan();
                return Function.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected R UseReadOnlySpan<R>(int Count, Func<ReadOnlySpan<T>, R> Function)
        {
            return UseReadOnlySpan(0, Count, Function);
        }

        protected R UseReadOnlySpan<R>(int Start, int Count, Func<ReadOnlySpan<T>, R> Function)
        {
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> Span = AsSpan(Start, Count);
                return Function.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected R UseReadOnlySpan<I, R>(I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct where R : allows ref struct
        {
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> Span = AsSpan();
                return Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected R UseReadOnlySpan<I, R>(int Count, I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct where R : allows ref struct
        {
            return UseReadOnlySpan(0, Count, Other, Action);
        }

        protected R UseReadOnlySpan<I, R>(int Start, int Count, I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct where R : allows ref struct
        {
            Lock.EnterReadLock();
            try
            {
                ReadOnlySpan<T> Span = AsSpan(Start, Count);
                return Action.Invoke(Span, Other);
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

        private Span<T> AsSpan()
        {
            int Limit = GetAbsoluteLimit();
            return Source.AsSpan(this, 0, Limit);
        }

        private Span<T> AsSpan(int Start, int Count)
        {
            int Limit = GetAbsoluteLimit();

            if ((uint)Start > (uint)Limit || (uint)Count > (uint)(Limit - Start))
            {
                throw new IndexOutOfRangeException($"Index out of range: Start(={Start}); Count(={Count}); Limit(={Limit})");
            }

            return Source.AsSpan(this, Start, Count);
        }


        private int GetAbsoluteLimit()
        {
            int Limit  = GetSpanLimit();
            int Length = this.Length;

            return Limit < 0 || Limit > this.Length
                ? Length
                : Limit;
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