using System.Collections;
using System.Runtime.CompilerServices;

namespace Zion
{
    public enum CompareMode : sbyte
    {
        MinToMax = 1,
        MaxToMin = -1
    }

    public class SortedList<T> : ICollection<T>, IBinaryGeneric<SortedList<T>, T> where T : IComparable<T>
    {
        public CompareMode CompareMode { get; init; } = CompareMode.MinToMax;

        private T[] Data;
        private int Length;

        private readonly bool IsReference = RuntimeHelpers.IsReferenceOrContainsReferences<T>();

        public int Capacity => Data.Length;
        public int Count => Length;
        public bool IsReadOnly => false;

        public SortedList() : this(16) { }
        public SortedList(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Capacity);

            Data = new T[Capacity];
            Length = 0;
        }
        private SortedList(T[] Data, int Count, int CompareMode)
        {
            this.Data = Data;
            Length = Count;
            this.CompareMode = (CompareMode)CompareMode;
            Update();
        }


        public T this[int Index] => Data[Index];
        public T this[Index Index] => Data[Index];
        public T[] this[Range Range] => Data[Range];


        public static implicit operator T[](SortedList<T> RecordList)
        {
            T[] Array = new T[RecordList.Count];
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = RecordList[i];
            }
            return Array;
        }
        public static implicit operator List<T>(SortedList<T> RecordList)
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
            return this.ToEnumerableString();
        }

        public override bool Equals(object? Object)
        {
            if (Object is null) { return false; }
            if (Object is SortedList<T> RecordList)
            {
                if (RecordList.Count != Count)
                {
                    return false;
                }
                for (int i = 0; i < Count; i++)
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

        public override int GetHashCode()
        {
            HashCode Hash = new HashCode();
            foreach (T Item in Data)
            {
                Hash.Add(Item);
            }
            Hash.Add(CompareMode);
            return Hash.ToHashCode();
        }


        public void Add(T Item)
        {
            EnsureCapacity(Length + 1);
            int InsertIndex = GetInsertIndex(Item);
            Insert(Item, InsertIndex);
        }

        public void AddRange(params ICollection<T>[] Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            int TotalItems = Items.Sum(collection => collection?.Count ?? 0);
            EnsureCapacity(Length + TotalItems);

            foreach (var Collection in Items)
            {
                if (Collection is null) { continue; }

                foreach (var Item in Collection)
                {
                    int InsertIndex = GetInsertIndex(Item);
                    Insert(Item, InsertIndex);
                }
            }
        }

        public void Clear()
        {
            if (Length == 0)
            {
                return;
            }

            if (IsReference)
            {
                Array.Clear(Data, 0, Length);
            }

            Length = 0;
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

            int Min = 0;
            int Max = Length - 1;

            while (Min <= Max)
            {
                int Target = Min + ((Max - Min) >> 1);
                int Comparing = Item.CompareTo(Data[Target]);

                if (CompareMode == CompareMode.MaxToMin)
                {
                    Comparing = -Comparing;
                }

                if (Comparing == 0)
                {
                    return Target;
                }
                if (int.IsNegative(Comparing))
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
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfNegative(ArrayIndex);
            ArgumentException.ThrowIf(Array.Length - ArrayIndex < Length, "Target array is too small");

            System.Array.Copy(Data, 0, Array, ArrayIndex, Length);
        }

        public bool Remove(T Item)
        {
            int Index = IndexOf(Item);
            if (Index == -1) { return false; }

            RemoveAt(Index);

            return true;
        }

        public void RemoveAt(int Index)
        {
            if ((uint)Index >= (uint)Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Index));
            }

            for (int i = Index; i < Length - 1; i++)
            {
                Data[i] = Data[i + 1];
            }

            Length--;
            if (IsReference)
            {
                Data[Length] = default!;
            }
        }

        public void RemoveRange(int Start, int Count)
        {
            if (Start >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Start));
            }
            if (Count < 0 || Start + Count > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Count));
            }

            int End = Start + Count;

            foreach (int i in ZEnumerable.Range(Length - End))
            {
                Data[Start + i] = Data[End + i];
            }

            if (IsReference)
            {
                Array.Clear(Data, Length - Count, Count);
            }

            Length -= Count;
        }


        public void EnsureCapacity(int MinCapacity)
        {
            if (MinCapacity <= Data.Length) return;

            int NewCapacity = Math.Max(Data.Length * 2, MinCapacity);
            Array.Resize(ref Data, NewCapacity);
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


        public void Update()
        {
            Array.Sort(Data, 0, Length);

            if (CompareMode == CompareMode.MaxToMin)
            {
                Array.Reverse(Data, 0, Length);
            }
        }


        public void Write(BinaryWriter Writer, Action<BinaryWriter, T> WriteObject)
        {
            Writer.Write(Length);
            Writer.Write((sbyte)CompareMode);

            for (int i = 0; i < Length; i++)
            {
                WriteObject(Writer, Data[i]);
            }
        }

        public static SortedList<T> Read(BinaryReader Reader, Func<BinaryReader, T> ReadObject)
        {
            int Length = Reader.ReadInt32();
            int CompareModeValue = Reader.ReadSByte();
            T[] Data = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                Data[i] = ReadObject(Reader);
            }

            return new SortedList<T>(Data, Length, CompareModeValue);
        }


        public T[] ToArray()
        {
            T[] Array = new T[Length];
            System.Array.Copy(Data, Array, Length);
            return Array;
        }

        public List<T> ToList()
        {
            List<T> List = new List<T>(Length);
            foreach (T Item in this)
            {
                List.Add(Item);
            }
            return List;
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
                int Target = Min + ((Max - Min) >> 1);
                int Comparing = Item.CompareTo(Data[Target]);

                if (CompareMode == CompareMode.MaxToMin)
                {
                    Comparing = -Comparing;
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
            if (Length == Data.Length)
            {
                EnsureCapacity(Length + 1);
            }

            for (int i = Length; i > Index; i--)
            {
                Data[i] = Data[i - 1];
            }

            Data[Index] = Item;
            Length++;
        }
    }
}