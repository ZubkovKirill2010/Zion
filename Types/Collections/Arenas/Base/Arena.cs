namespace Zion
{
    public sealed class Arena<T>
    {
        private T[] Data;

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
            Capacity = int.Max(64, Capacity);
            Data = new T[Capacity];
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
            return new(Allocate(Size));
        }

        public ArenaList<T> GetList(int Size)
        {
            return new(Allocate(Size));
        }

        public ArenaQueue<T> GetQueue(int Size)
        {
            return new(Allocate(Size));
        }


        public ArenaSpan<T> Allocate(int Size)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);
            //TODO
            throw new NotImplementedException();
        }


        public ReadOnlySpan<T> AsSpan()
        {
            return new ReadOnlySpan<T>(Data);
        }

        public ReadOnlySpan<T> AsSpan(int Start, int Length)
        {
            return new ReadOnlySpan<T>(Data, Start, Length);
        }


        public T[] ToArray()
        {
            return ZArray.Clone(Data);
        }

        public T[] ToArray(int Start, int Length)
        {
            return ZArray.GetSubArray(Data, Start, Length);
        }


        internal Span<T> GetSpan(int Start, int Count)
        {
            return new Span<T>(Data, Start, Count);
        }


        internal void Release(ArenaSpan<T> Span)
        {
            ArgumentNullException.ThrowIfNull(Span);

            //TODO
            throw new NotImplementedException();
        }

        internal IEnumerator<T> GetEnumerator(int Start, int Count)
        {
            int End = Start + Count;
            for (int i = Start; i < End; i++)
            {
                yield return Data[i];
            }
        }
    }
}