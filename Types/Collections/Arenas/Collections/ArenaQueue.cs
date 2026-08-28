namespace Zion
{
    public sealed class ArenaQueue<T> : ArenaCollection<T>, ICollection<T>
    {
        public bool IsReadOnly => false;

        public int Count { get; private set; }


        public ArenaQueue(ArenaSpan<T> Data) : base(Data) { }


        public void Enqueue(T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public T Peek()
        {
            throw new NotImplementedException(); //TODO
        }

        public T Dequeue()
        {
            throw new NotImplementedException(); //TODO
        }


        public bool TryPeek(out T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public bool TryDequeue(out T Item)
        {
            throw new NotImplementedException(); //TODO
        }
        

        public void TrimExcess()
        {
            throw new NotImplementedException(); //TODO
        }

        public void TrimExcess(int Capacity)
        {
            throw new NotImplementedException(); //TODO
        }


        public void Add(T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public bool Contains(T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            throw new NotImplementedException(); //TODO
        }

        public bool Remove(T Item)
        {
            throw new NotImplementedException(); //TODO
        }

        public void Clear()
        {
            throw new NotImplementedException(); //TODO
        }


        public void EnsureCapacity(int NewCapacity)
        {
            throw new NotImplementedException(); //TODO
        }


        public T[] ToArray()
        {
            throw new NotImplementedException(); //TODO
        }

        public Stack<T> ToQueue()
        {
            throw new NotImplementedException(); //TODO
        }

        public List<T> ToList()
        {
            throw new NotImplementedException(); //TODO
        }


        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException(); //TODO
        }
    }
}