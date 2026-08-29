namespace Zion
{
    public sealed class Arena<T>
    {
        internal const int BinaryGroupSize = 4;
        internal const int GroupSize = 1 << BinaryGroupSize;

        internal const int BinaryBufferAccuracry = 9;
        internal const int BufferAccuracry = 1 << BinaryBufferAccuracry;


        private T[] Data;
        private BitArray BitMap;
        private int Count;

        public int Capacity
        {
            get => Data.Length;
            set
            {
                if (value > Data.Length)
                {
                    value = RoundToBufferSize(value);
                    Array.Resize(ref Data, value);
                    BitMap = BitArray.Resize(BitMap, value >> BinaryGroupSize);
                }
            }
        }


        public Arena() : this(1024) { }

        public Arena(int Capacity)
        {
            Capacity = RoundToBufferSize(Math.Max(1024, Capacity));
            this.Data     = new T[Capacity];
            this.BitMap   = new (RoundToGroup(Capacity));
        }


        internal T this[int Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }

        internal T this[Index Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }


        public ArenaArray<T> GetArray(int Size)
        {
            return new(Allocate(Size));
        }

        public ArenaBuffer<T> GetBuffer(int Size)
        {
            return new(Allocate(RoundToGroup(Size)));
        }

        public ArenaList<T> GetList(int Size)
        {
            return new(Allocate(RoundToGroup(Size)));
        }

        public ArenaQueue<T> GetQueue(int Size)
        {
            return new(Allocate(RoundToGroup(Size)));
        }

        public ArenaSpan<T> Allocate(int Size)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);

            int Start = GetFreeArea(Size);

            MarkArea(Start, Size, true);
            UpdateCount(Start + Size);

            return new ArenaSpan<T>(this, Start, Size);
        }


        public T[] ToArray()
        {
            return ZArray.Clone(Data);
        }

        public T[] ToArray(int Start, int Length)
        {
            return ZArray.GetSubArray(Data, Start, Length);
        }


        internal Span<T> AsSpan(ArenaSpan<T> ArenaSpan)
        {
            CheckSpan(ArenaSpan);
            return Data.AsSpan(ArenaSpan.Start, ArenaSpan.Count);
        }

        internal Span<T> AsSpan(ArenaSpan<T> ArenaSpan, int Start, int Count)
        {
            CheckSpan(ArenaSpan);

            int ArenaLength = ArenaSpan.Count;

            ArgumentOutOfRangeException.ThrowIfWithout(Start, ArenaLength);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, ArenaLength);

            return Data.AsSpan(ArenaSpan.Start + Start, Count);
        }

        internal void Release(ArenaSpan<T> Span)
        {
            CheckSpan(Span);
            MarkArea(Span.Start, Span.Count, false);

            if (IsLastSpan(Span))
            {
                Count = Span.Start;
            }
        }

        internal ArenaSpan<T> Expand(ArenaSpan<T> Span, int Count)
        {
            if (TryExpand(Span, Count, out ArenaSpan<T> Expanded))
            {
                return Expanded;
            }

            ArenaSpan<T> Allocated = Allocate(Count);
            CopyTo(Span, Allocated);

            MarkArea(Span, false);

            return Allocated;
        }

        internal IEnumerator<T> GetEnumerator(int Start, int Count)
        {
            int End = Start + Count;
            for (int i = Start; i < End; i++)
            {
                yield return Data[i];
            }
        }


        private void CheckSpan(ArenaSpan<T> Span)
        {
            if (!ReferenceEquals(this, Span.Source))
            {
                throw new InvalidOperationException("Arena not contains this ArenaSpan");
            }
            if (Span.IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Span));
            }
        }

        private void UpdateCount(int NewCount)
        {
            if (NewCount > Count)
            {
                Count = RoundToGroup(NewCount);
            }
        }

        private void MarkArea(ArenaSpan<T> Span, bool Busy)
        {
            MarkArea(Span.Start, Span.Count, Busy);
        }

        private void MarkArea(int Start, int Count, bool Busy)
        {
            BitMap.Fill(FloorToGroup(Start), RoundToGroup(Count), Busy);
        }

        private void CopyTo(ArenaSpan<T> Source, ArenaSpan<T> Destination)
        {
            Data.AsSpan(Source.Start, Source.Count)
                .CopyTo(Data.AsSpan(Destination.Start, Source.Count));
        }

        private bool IsLastSpan(ArenaSpan<T> Span)
        {
            return Span.Start + Span.Count == Count;
        }

        private bool TryExpand(ArenaSpan<T> Span, int Additional, out ArenaSpan<T> Expanded)
        {
            int SpanEnd = Span.Start + Span.Count;
            int Start = RoundToGroup(SpanEnd);
            int End = RoundToGroup(SpanEnd + Additional);
              
            if (Start == End || !BitMap.Contains(Start, End - Start, true))
            {
                Expanded = new ArenaSpan<T>(this, Span.Start, SpanEnd + Additional);
                return true;
            }

            Expanded = default!;
            return false;
        }

        private int GetFreeArea(int Size)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);
            if (Size == 0)
            {
                return Count;
            }

            if (BitMap.TryFindShortestSequence(RoundToGroup(Size), false, Count, out Segment Sequence))
            {
                return Sequence.Start;
            }

            Capacity += Size;
            return Count;
        }


        public static int RoundToGroup(int Count)
        {
            return (Count + GroupSize - 1) >> BinaryGroupSize;
        }

        public static int FloorToGroup(int Count)
        {
            return Count >> BinaryGroupSize;
        }

        public static int RoundToBufferSize(int Count)
        {
            return (Count + BufferAccuracry - 1) & ~(BufferAccuracry - 1);
        }
    }
}