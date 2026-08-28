namespace Zion
{
    public sealed class ArenaBuffer<T> : ArenaCollection<T>
    {
        public int Capacity { get; }

        public int Count { get; private set; }


        public ArenaBuffer(ArenaSpan<T> Data) : base(Data) { }
        
        
        //public T this[int Index]
        //{
        //    get => Data[Index];
        //    set => Data[Index] = value;
        //}

        //public T this[Index Index]
        //{
        //    get => Data[Index];
        //    set => Data[Index] = value;
        //}


        public void Add(T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public bool TryAdd(T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public void AddRange(IEnumerable<T> Items)
        {
            throw new NotImplementedException(); //TODO
        }

        public void AddRange(ReadOnlySpan<T> Items)
        {
            throw new NotImplementedException(); //TODO
        }


        public bool Remove(T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public void RemoveAt(int Index)
        {
            throw new NotImplementedException(); //TODO
        }

        public void RemoveRange(int Index, int Count)
        {
            throw new NotImplementedException(); //TODO
        }

        public void Clear()
        {
            throw new NotImplementedException(); //TODO
        }


        public T Peek()
        {
            throw new NotImplementedException(); //TODO
        }

        public T First()
        {
            throw new NotImplementedException(); //TODO
        }

        public T Last()
        {
            throw new NotImplementedException(); //TODO
        }


        public int IndexOf(T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public bool Contains(T Item)
        {
            throw new NotImplementedException(); //TODO
        }


        public void Insert(int Index, T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public void InsertRange(int Index, IEnumerable<T> Items)
        {
            throw new NotImplementedException(); //TODO
        }

        public void Reverse()
        {
            throw new NotImplementedException(); //TODO
        }

        public void Sort()
        {
            throw new NotImplementedException(); //TODO
        }


        public T[] ToArray()
        {
            throw new NotImplementedException(); //TODO
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            throw new NotImplementedException(); //TODO
        }

        public void CopyTo(ArenaBuffer<T> Destination)
        {
            throw new NotImplementedException(); //TODO
        }


        public void EnsurceCapacity(int Capacity)
        {
            throw new NotImplementedException(); //TODO
        }

        public void Resize(int NewSize)
        {
            throw new NotImplementedException(); //TODO
        }

        public void TrimExcess()
        {
            throw new NotImplementedException(); //TODO
        }


        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException(); //TODO
        }
    }
}