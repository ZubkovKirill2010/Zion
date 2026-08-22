namespace Zion
{
    public sealed class ArenaArray<T> : ArenaCollection<T>
    {
        public ArenaArray(ArenaSpan<T> Data) : base(Data) { }


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


        public void Clear()
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


        public override IEnumerator<T> GetEnumerator()
        {

        }
    }
}