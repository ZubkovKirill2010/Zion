namespace Zion
{
    public sealed class Arena<T>
    {
        private const int BinaryGroupSize = 4;
        private const int GroupSize = 1 << BinaryGroupSize; //8

        private T[] Data;
        private BitArray BitMap;
        private int NextFree;

        public int Capacity
        {
            get => Data.Length;
            set
            {
                if (value > Data.Length)
                {
                    Array.Resize(ref Data, value);
                }
            }
        }


        public Arena() : this(1024) { }

        public Arena(int Capacity)
        {
            Capacity = Math.Max(64, Capacity);
            this.Capacity = Capacity;
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
        //TODO: Искать пустое пространство через BitMap
        public ArenaSpan<T> Allocate(int Size)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);

            ArenaSpan<T> Span = new ArenaSpan<T>(this, NextFree, Size);

            MarkArea(NextFree, Size, true);
            NextFree = RoundToGroup(NextFree + Size);

            return Span;
        }


        public T[] ToArray()
        {
            return ZArray.Clone(Data);
        }

        public T[] ToArray(int Start, int Length)
        {
            return ZArray.GetSubArray(Data, Start, Length);
        }


        public ReadOnlySpan<T> AsSpan()
        {
            return new ReadOnlySpan<T>(Data);
        }

        public ReadOnlySpan<T> AsSpan(int Start, int Length)
        {
            return new ReadOnlySpan<T>(Data, Start, Length);
        }


        internal void Release(ArenaSpan<T> Span)
        {
            CheckSpan(Span);
            MarkArea(Span.Start, Span.Count, false);

            if (IsLastSpan(Span))
            {
                NextFree = Span.Start;
            }
        }

        internal ArenaSpan<T> Expand(ArenaSpan<T> Span, int Count)
        {
            //TODO
            //Если справа от Span есть пустое место, то возвращаем тот же участок
            //Если нет то копируем данные на новое место и возвращаем новый участок

            throw new NotImplementedException();

            if (IsLastSpan(Span))
            {
                Capacity = Span.Start + Count;
                return new ArenaSpan<T>(this, Span.Start, Count);
            }

            MarkArea(Span.Start, Span.Count, false);

            ArenaSpan<T> New = Allocate(Count);
            Array.Copy(Data, Span.Start, Data, New.Start, Span.Count);

            return New;
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
            if (Span is null) { throw new ArgumentNullException(nameof(Span)); }
            if (Span.IsDisposed) { throw new ObjectDisposedException(nameof(Span)); }
            if (!Span.IsFrom(this)) { throw new InvalidOperationException("Arena not contains this span"); }
        }

        private void MarkArea(int Start, int Count, bool Busy)
        {
            BitMap.Fill(Start >> BinaryGroupSize, Count, Busy);
        }

        private int GetFreeArea(int Count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Count);
            if (Count == 0)
            {
                return NextFree;
            }

            //TODO: Вернуть начальную позицию для ArenaSpan размером Count

            return -1;
        }

        private bool IsLastSpan(ArenaSpan<T> Span)
        {
            return Span.Start + Span.Count == NextFree;
        }


        private static int RoundToGroup(int Count)
        {
            return (Count + GroupSize - 1) >> BinaryGroupSize;
        }
    }
}