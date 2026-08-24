using System.Collections.ObjectModel;

namespace Zion
{
    public sealed class ArenaList<T> : ArenaCollection<T>, IList<T>
    {
        public int Count { get; private set; }

        public bool IsReadOnly => false;


        public ArenaList(ArenaSpan<T> Data) : base(Data) { }


        public T this[int Index]
        {
            get
            {
                ThrowIfWithout(Index);
                return Data[Index];
            }
            set
            {
                ThrowIfWithout(Index);
                Data[Index] = value;
            }

        }

        public T this[Index Index]
        {
            get
            {
                ThrowIfWithout(Index.GetOffset(Count));
                return Data[Index];
            }
            set
            {
                ThrowIfWithout(Index.GetOffset(Count));
                Data[Index] = value;
            }

        }


        public void Add(T Item)
        {
            throw new NotImplementedException();
        }

        public void AddRange()
        {

        }

        public void Insert(int Index, T Item)
        {
            throw new NotImplementedException();
        }

        public void InsertRange(int Index, IEnumerable<T> collection)
        {

        }



        public int IndexOf(T Item)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T Item, int Index)
        {

        }

        public int IndexOf(T Item, int Index, int Count)
        {

        }


        public int FindIndex(int Start, int Count, Predicate<T> Match)
        {

        }

        public int FindIndex(int Start, Predicate<T> Match)
        {

        }

        public int FindIndex(Predicate<T> Match)
        {

        }


        public T Find(Predicate<T> Match)
        {

        }

        public T? FindLast(Predicate<T> Match)
        {

        }

        public List<T> FindAll(Predicate<T> Match)
        {

        }


        public int LastIndexOf(T Item)
        {

        }

        public int LastIndexOf(T Item, int Index)
        {

        }

        public int LastIndexOf(T Item, int Index, int Count)
        {

        }


        public int FindLastIndex(int startIndex, int Count, Predicate<T> Match)
        {

        }

        public int FindLastIndex(int startIndex, Predicate<T> Match)
        {

        }

        public int FindLastIndex(Predicate<T> Match)
        {

        }


        public bool Exists(Predicate<T> Match)
        {

        }

        public bool Contains(T Item)
        {
            throw new NotImplementedException();
        }


        public void CopyTo(T[] Array)
        {

        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(int Index, T[] Array, int ArrayIndex, int Count)
        {

        }


        public void RemoveAt(int Index)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(int Index, int Count)
        {

        }

        public bool Remove(T Item)
        {
            throw new NotImplementedException();
        }

        public int RemoveAll(Predicate<T> Match)
        {

        }

        public void Clear()
        {
            throw new NotImplementedException();
        }


        public void Reverse(int Index, int Count)
        {

        }

        public void Reverse()
        {

        }


        public void Sort()
        {

        }

        public void Sort(IComparer<T>? Comparer)
        {

        }

        public void Sort(Comparison<T> Comparison)
        {

        }

        public void Sort(int Index, int Count, IComparer<T>? Comparer)
        {

        }


        public int BinarySearch(T Item)
        {

        }

        public int BinarySearch(T Item, IComparer<T> Comparer)
        {

        }

        public int BinarySearch(int Index, int Count, T Item, IComparer<T> Comparer)
        {

        }


        public List<T> GetRange(int Index, int Count)
        {

        }

        public List<T> Slice(int Start, int Length)
        {

        }


        public void ForEach(Action<T> Action)
        {

        }

        public bool TrueForAll(Predicate<T> Match)
        {

        }

        public List<TOut> ConvertAll<TOut>(Converter<T, TOut> Converter)
        {

        }


        public void EnsurceCapacity(int Capacity)
        {

        }

        public void TrimExcess()
        {

        }


        public T[] ToArray()
        {

        }

        public List<T> ToList()
        {

        }

        public ReadOnlyCollection<T> AsReadOnly()
        {

        }


        public override IEnumerator<T> GetEnumerator()
        {
            
        }


        private void ThrowIfWithout(int Index)
        {

        }
    }
}