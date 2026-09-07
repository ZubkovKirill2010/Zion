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
                Data.Modify();
            }
        }

        public int Capacity => Data.Count;


        public ArenaBuffer(ArenaSpan<T> Data) : base(Data) { }
        
        
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
            get => this[Index.GetOffset(Count)];
            set => this[Index.GetOffset(Count)] = value;
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
                Data.Expand(TotalCount);
                Data.UseSpan
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

            Data.Expand(TotalCount);
            Data.UseSpan
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

            Data.Move(Index + 1, Index, Count - Index - 1);
            Count--;
        }

        public void RemoveRange(int Index, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Index, this.Count);
            ArgumentOutOfRangeException.ThrowIfWithout(Index + Count, this.Count);

            Data.Move(Index + Count, Index, this.Count - Count - Index);
            this.Count -= Count;
        }

        public void Clear()
        {
            Data.UseSpan(Count, static Span => Span.Clear());
        }


        public T First()
        {
            return Data[0];
        }

        public T Last()
        {
            return Data[Count - 1];
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return Data.UseSpan(Count, Span => Span.IndexOf(Item, Comparer));
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return Data.UseSpan(Count, Span => Span.Contains(Item, Comparer));
        }


        public void Insert(int Index, T Item)
        {
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Count);

            Data.Move(Index, Index + 1, Count - Index);
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
                Data.Expand(TotalCount);

                if (Index < Count)
                {
                    Data.Move(Index, Index + ItemsCount, Count - Index);
                }

                Data.UseSpan
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
                using var Tail = Data.Source.GetArray(TailLength);

                if (TailLength > 0)
                {
                    Tail.UseSpan(Span => Data.CopyTo(Index, Count - Index, Span));
                }

                InsertItems(Index, Items, out int TotalCount);

                Data.Expand(TotalCount + TailLength);
                Data.UseSpan(TotalCount, TailLength, Tail.CopyTo);

                Count = TotalCount + TailLength;
            }
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
            return Data.ToArray(0, Count);
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(ArenaBuffer<T> Destination)
        {
            Destination.UseSpan(Data.CopyTo);
        }

        public void CopyTo(Span<T> Destination)
        {
            Data.CopyTo(Destination);
        }


        public void EnsurceCapacity(int Capacity)
        {
            Data.Expand(Capacity);
        }

        public void Resize(int NewSize)
        {
            Count = Math.Max(0, NewSize);
        }

        public void TrimExcess()
        {
            throw new NotImplementedException(); //TODO
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

                Data.Expand(TotalIndex + LocalIndex);
                Data.UseSpan
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