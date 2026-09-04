namespace Zion
{
    public struct ArenaSpan<T> : IDisposable//, IEnumerable<T>
    {
        #region Data
        private readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.SupportsRecursion);

        internal readonly Arena<T> Source;

        public readonly int Start;
        public readonly int Count;

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

        #region PublicMethods
        public void Use(Action<Span<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
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

        public void Use(int Count, Action<Span<T>> Action)
        {
            Use(0, Count, Action);
        }

        public void Use(int Start, int Count, Action<Span<T>> Action)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
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

        public I Use<I>(Func<Span<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
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

        public I Use<I>(int Count, Func<Span<T>, I> Function)
        {
            return Use(0, Count, Function);
        }

        public I Use<I>(int Start, int Count, Func<Span<T>, I> Function)
        {
            ThrowIfDisposed();
            Lock.EnterReadLock();
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


        public ArenaSpan<T> Expand(int Capacity)
        {
            ThrowIfDisposed();

            if (Capacity <= Count)
            {
                return this;
            }

            Lock.EnterWriteLock();
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
            if (Count <= 0 || SourceIndex == DestinationIndex) return;

            ThrowIfWithout(SourceIndex);
            ThrowIfWithout(SourceIndex + Count - 1);
            ThrowIfWithout(DestinationIndex);
            ThrowIfWithout(DestinationIndex + Count - 1);

            Lock.EnterWriteLock();
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

        //#region IEnumerable
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //public IEnumerator<T> GetEnumerator()
        //{
        //    ThrowIfDisposed();
        //    Lock.EnterReadLock();
        //    try
        //    {
        //        return Source.GetEnumerator(Start, Count);
        //    }
        //    finally
        //    {
        //        Lock.ExitReadLock();
        //    }
        //}

        //public IEnumerator<T> GetEnumerator(int Start, int Length)
        //{
        //    ThrowIfDisposed();
        //    ArgumentOutOfRangeException.ThrowIfWithout(Start, Count);
        //    ArgumentOutOfRangeException.ThrowIfWithout(Start + Length, Count);

        //    Lock.EnterReadLock();
        //    try
        //    {
        //        return Source.GetEnumerator(this.Start, Length);
        //    }
        //    finally
        //    {
        //        Lock.ExitReadLock();
        //    }            
        //}

        //#endregion

        #region IDisposable
        public void Dispose()
        {
            if (IsDisposed) { return; }

            Lock.EnterWriteLock();
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
    }
}