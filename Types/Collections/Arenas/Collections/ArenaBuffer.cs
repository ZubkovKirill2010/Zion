namespace Zion
{
    public sealed class ArenaBuffer<T> : ArenaCollection<T>
    {
        public int Count
        {
            get;
            private set
            {
                field = value;
                Modify();
            }
        }

        public int Capacity => Count;


        public ArenaBuffer(ArenaSpan<T> Data) : base(Data) { }
        
        
        public new T this[int Index]
        {
            get => base[Index];
            set => base[Index] = value;
        }

        public new T this[Index Index]
        {
            get => base[Index];
            set => base[Index] = value;
        }


        public void Add(T Item)
        {
            int Index = Count;
            EnsurceCapacity(++Count);
            this[Index] = Item;
        }

        public void AddRange(IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            int Count = this.Count;

            if (Items.TryGetNonEnumeratedCount(out int ItemsCount))
            {
                int TotalCount = Count + ItemsCount;
                Expand(TotalCount);
                UseSpan
                (
                    Count, ItemsCount, Span =>
                    {
                        int Index = 0;

                        foreach (T Item in Items)
                        {
                            Span[Index++] = Item;
                        }
                    }
                );
                this.Count = TotalCount;
            }
            else
            {
                InsertItems(Count, Items, out int NewCount);
                this.Count = NewCount;
            }
        }

        public void AddRange(ReadOnlySpan<T> Items)
        {
            int ItemsCount = Items.Length;
            int TotalCount = Count + Items.Length;

            Expand(TotalCount);
            UseSpan
            (
                Count, ItemsCount, Items,
                (Span, Other) =>
                {
                    for (int i = 0; i < ItemsCount; i++)
                    {
                        Span[i] = Other[i];
                    }
                }
            );
            Count = TotalCount;
        }


        public bool Remove(T Item)
        {
            int Index = IndexOf(Item);
            
            if (Index == -1)
            {
                return false;
            }

            RemoveAt(Index);
            return true;
        }

        public void RemoveAt(int Index)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);

            Move(Index + 1, Index, Count - Index - 1);
            Count--;
        }

        public void RemoveRange(int Index, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Index, this.Count);
            ArgumentOutOfRangeException.ThrowIfWithout(Index + Count, this.Count);

            Move(Index + Count, Index, this.Count - Count - Index);
            this.Count -= Count;
        }

        public void Clear()
        {
            UseSpan(Count, static Span => Span.Clear());
        }


        public T First()
        {
            return this[0];
        }

        public T Last()
        {
            return this[Count - 1];
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Count, Span => Span.IndexOf(Item, Comparer));
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Count, Span => Span.Contains(Item, Comparer));
        }


        public void Insert(int Index, T Item)
        {
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Count);

            Move(Index, Index + 1, Count - Index);
            this[Index] = Item;
            Count++;
        }

        public void InsertRange(int Index, IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Count);

            if (Items.TryGetNonEnumeratedCount(out int ItemsCount))
            {
                if (ItemsCount == 0)
                {
                    return;
                }

                int TotalCount = Count + ItemsCount;
                Expand(TotalCount);

                if (Index < Count)
                {
                    Move(Index, Index + ItemsCount, Count - Index);
                }

                UseSpan
                (
                    Index, ItemsCount, Span =>
                    {
                        int SpanIndex = 0;
                        foreach (T Item in Items)
                        {
                            Span[SpanIndex++] = Item;
                        }
                    }
                );

                Count = TotalCount;
            }
            else
            {
                int TailLength = Count - Index;
                using var Tail = Source.GetArray(TailLength);

                if (TailLength > 0)
                {
                    Tail.UseSpan(Span => CopyTo(Index, Count - Index, Span));
                }

                InsertItems(Index, Items, out int TotalCount);

                Expand(TotalCount + TailLength);
                UseSpan(TotalCount, TailLength, Tail.CopyTo);

                Count = TotalCount + TailLength;
            }
        }

        public void Reverse()
        {
            UseSpan(static Span => Span.Reverse());
        }

        public void Sort()
        {
            UseSpan(static Span => Span.Sort());
        }


        public T[] ToArray()
        {
            return ToArray(0, Count);
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
            return base.UseSpan(Count, Function);
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
            return base.UseReadOnlySpan(Function);
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


        public void EnsurceCapacity(int Capacity)
        {
            Expand(Capacity);
        }

        public void Resize(int NewSize)
        {
            Count = Math.Max(0, NewSize);
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(ArenaBuffer<T> Destination)
        {
            Destination.UseSpan(CopyTo);
        }

        public new void CopyTo(Span<T> Destination)
        {
            CopyTo(Destination);
        }


        protected override IEnumerator<int> GetIndexEnumerator()
        {
            int Count = this.Count;
            for (int i = 0; i < Count; i++)
            {
                yield return i;
            }
        }


        private void InsertItems(int Index, IEnumerable<T> Items, out int Count)
        {
            const int BufferSize = 0x20;

            T[] Buffer = new T[BufferSize];
            int LocalIndex = 0;
            int TotalIndex = Index;

            void Flush()
            {
                if (LocalIndex == 0)
                {
                    return;
                }

                Expand(TotalIndex + LocalIndex);
                UseSpan
                (
                    TotalIndex, LocalIndex, Span =>
                    {
                        for (int i = 0; i < LocalIndex; i++)
                        {
                            Span[i] = Buffer[i];
                        }
                    }
                );

                TotalIndex += LocalIndex;
                LocalIndex = 0;
            }

            foreach (T Item in Items)
            {
                Buffer[LocalIndex++] = Item;

                if (LocalIndex >= BufferSize)
                {
                    Flush();
                }
            }

            Flush();
            Count = TotalIndex;
        }
    }
}