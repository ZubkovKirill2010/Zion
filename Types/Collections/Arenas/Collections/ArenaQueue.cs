namespace Zion
{
    public sealed class ArenaQueue<T> : ArenaCollection<T>, ICollection<T>
    {
        public bool IsReadOnly => false;

        public int Count { get; private set; }


        public ArenaQueue(ArenaSpan<T> Data) : base(Data) { }


        public void Enqueue(T Item)
        {

        }

        public T Peek()
        {

        }

        public T Dequeue()
        {

        }


        public bool TryPeek(out T Item)
        {

        }

        public bool TryDequeue(out T Item)
        {

        }
        

        public void TrimExcess()
        {

        }

        public void TrimExcess(int Capacity)
        {

        }


        public void Add(T Item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T Item)
        {

        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T Item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {

        }


        public void EnsureCapacity(int NewCapacity)
        {

        }


        public T[] ToArray()
        {

        }

        public Stack<T> ToQueue()
        {
            Stack<T> Result = new(Count);
        }

        public List<T> ToList()
        {

        }


        public override IEnumerator<T> GetEnumerator()
        {
            //TODO
        }
    }
}