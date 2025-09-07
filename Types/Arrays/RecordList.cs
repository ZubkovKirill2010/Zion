using System.Collections;

namespace Zion
{
    public enum CompareMode : sbyte
    {
        MinToMax = 1,
        MaxToMin = -1
    }

    public class RecordList<T> : ICollection<T>, IBinaryGeneric<RecordList<T>, T> where T : IComparable<T>
    {
        private readonly CompareMode CompareMode;

        private readonly T[] Data;
        private int Length;

        public int Capacity    => Data.Length;
        public int Count       => Length;
        public bool IsReadOnly => false;

        public RecordList(int Capacity, CompareMode CompareMode)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Capacity);

            this.CompareMode = CompareMode;
            Data = new T[Capacity];
            Length = 0;
        }
        private RecordList(T[] Data, int Count, int CompareMode)
        {
            this.Data = Data;
            Length = Count;
        }


        public T this[int Index] => Data[Index];
        public T this[Index Index] => Data[Index];
        public T[] this[Range Range] => Data[Range];


        public static implicit operator T[](RecordList<T> RecordList)
        {
            T[] Array = new T[RecordList.Count];
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = RecordList[i];
            }
            return Array;
        }
        public static implicit operator List<T>(RecordList<T> RecordList)
        {
            List<T> List = new List<T>(RecordList.Count);
            
            foreach (T Item in RecordList)
            {
                List.Add(Item);
            }

            return List;
        }


        public override string ToString()
        {
            return $"[{Data.ToEnumerableString()}]";
        }
        public override bool Equals(object? Object)
        {
            if (Object is null) { return false; }
            if (Object is RecordList<T> RecordList)
            {
                if (RecordList.Count != Count)
                {
                    return false;
                }
                for (int i = 0; i < Data.Length; i++)
                {
                    if (!RecordList.Data[i].Equals(Data[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
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

        public bool Contains(T Item)
        {
            return IndexOf(Item) != -1;
        }

        public int IndexOf(T Item)
        {
            if (Length == 0)
            {
                return -1;
            }
            if (Length == 1 && Item.CompareTo(Data[0]) == 0)
            {
                return 0;
            }

            int Min = 0;
            int Max = Length - 1;

            while (Min <= Max)
            {
                int Target = Min + ((Max - Min) / 2);
                int Comparing = Item.CompareTo(Data[Target]);

                if (CompareMode == CompareMode.MaxToMin)
                {
                    Comparing *= -1;
                }

                if (Comparing == 0)
                {
                    return Target;
                }

                if (Comparing < 0)
                {
                    Max = Target - 1;
                }
                else
                {
                    Min = Target + 1;
                }
            }

            return -1;
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


        public void Write(BinaryWriter Writer, Action<BinaryWriter, T> WriteObject)
        {
            Writer.Write(Data, WriteObject);
            Writer.Write(Length);
            Writer.Write((sbyte)CompareMode);
        }

        public static RecordList<T> Read(BinaryReader Reader, Func<BinaryReader, T> ReadObject)
        {
            return new RecordList<T>
            (
                Reader.ReadArray(ReadObject),
                Reader.ReadInt32(),
                Reader.ReadSByte()
            );
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
                int Comparing = Item.CompareTo(Data[Target]);

                if (CompareMode == CompareMode.MaxToMin)
                {
                    Comparing *= -1;
                }

                if (Comparing == 0)
                {
                    return Target;
                }

                if (Comparing < 0)
                {
                    Max = Target - 1;
                }
                else
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