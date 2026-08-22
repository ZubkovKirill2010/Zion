namespace Zion
{
    public sealed class ArenaStack<T> : ArenaCollection<T>, ICollection<T>
    {
        public bool IsReadOnly => false;

        public int Count { get; private set; }


        public ArenaStack(ArenaSpan<T> Data) : base(Data) { }


        public void Push(T Item)
        {
            
        }

        public T Pop()
        {

        }

        public T Peek()
        {

        }


        public bool TryPop(out T Item)
        {

        }

        public bool TryPeek(out T Item)
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

        public Stack<T> ToStack()
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