using System.Collections;

namespace Zion
{
    public class RecordList<T> : ICollection<T>
    {
        private readonly Func<T, T, int> Comparer;

        private readonly T[] Data;
        private int Length;

        public int Capacity    => Data.Length;
        public int Count       => Length;
        public bool IsReadOnly => false;

        public RecordList(int Capacity, Func<T, T, int> Comparer)
        {
            if (Capacity <= 0)
            {
                throw new ArgumentOutOfRangeException($"Capacity(={Capacity}) <= 0)");
            }

            this.Comparer = Comparer;

            Data = new T[Capacity];
            Length = 0;
        }


        public void Add(T Item)
        {
            int InsertIndex = GetInsertIndex(Item);

            if (Length < Capacity)
            {
                Insert(Item, InsertIndex);
                return;
            }

            if (InsertIndex < Capacity)
            {
                for (int i = Capacity - 1; i > InsertIndex; i--)
                {
                    Data[i] = Data[i - 1];
                }
                Data[InsertIndex] = Item;
            }
        }

        public void Clear()
        {
            if (Length == 0)
            {
                return;
            }

            Length = 0;

            if (typeof(T).IsClass)
            {
                Array.Clear(Data, 0, Length);
            }
        }

        public bool Contains(T Target)
        {
            foreach (T Item in this)
            {
                if (Item.Equals(Target))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {

        }

        public bool Remove(T Item)
        {
            for (int i = 0; i < Length; i++)
            {
                if (Data[i].Equals(Item))
                {
                    int End = Length - 1;
                    int Index = i;

                    while (Index < End)
                    {
                        Data[Index++] = Data[Index];
                    }

                    return true;
                }
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return Data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private int GetInsertIndex(T Item)
        {
            if (Length == 0)
            {
                return 0;
            }

            int Min = 0;
            int Max = Length - 1;

            while (Min <= Max)
            {
                int Target = Min + ((Max - Min) / 2);
                int Comparing = Comparer(Item, Data[Target]);

                if (Comparing == 0)
                {
                    return Target;
                }

                if (Comparing < 0)
                {
                    Max = Target - 1;
                }
                else //Comparing > 0
                {
                    Min = Target + 1;
                }
            }

            return Min;
        }

        private void Insert(T Item, int Index)
        {
            for (int i = Length; i > Index; i--)
            {
                Data[i] = Data[i - 1];
            }

            Data[Index] = Item;
            Length++;
        }
    }
}