namespace Zion
{
    public sealed class ArenaBuffer<T> : ArenaCollection<T>
    {
        public int Capacity { get; }

        public int Count { get; private set; }


        public ArenaBuffer(ArenaSpan<T> Data) : base(Data) { }
        
        
        public T this[int Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }

        public T this[Index Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }


        public void Add(T Item)
        {

        }

        public bool TryAdd(T Item)
        {

        }

        public void AddRange(IEnumerable<T> Items)
        {

        }

        public void AddRange(ReadOnlySpan<T> Items)
        {

        }


        public bool Remove(T Item)
        {

        }

        public void RemoveAt(int Index)
        {

        }

        public void RemoveRange(int Index, int Count)
        {

        }

        public void Clear()
        {

        }


        public T Peek()
        {

        }

        public T First()
        {

        }

        public T Last()
        {

        }


        public int IndexOf(T Item)
        {

        }

        public bool Contains(T Item)
        {

        }


        public void Insert(int Index, T Item)
        {

        }

        public void InsertRange(int Index, IEnumerable<T> Items)
        {

        }

        public void Reverse()
        {

        }

        public void Sort()
        {

        }


        public T[] ToArray()
        {

        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {

        }

        public void CopyTo(ArenaBuffer<T> Destination)
        {

        }


        public void EnsurceCapacity(int Capacity)
        {

        }

        public void Resize(int NewSize)
        {

        }

        public void TrimExcess()
        {

        }


        public override IEnumerator<T> GetEnumerator()
        {

        }
    }
}