namespace Zion
{
    public sealed class ArenaArray<T> : ArenaCollection<T>
    {
        public ArenaArray(ArenaSpan<T> Data) : base(Data) { }


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


        public void Clear()
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


        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException(); //TODO
        }
    }
}