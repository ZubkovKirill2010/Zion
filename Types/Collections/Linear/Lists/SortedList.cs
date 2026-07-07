using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using Zion.Serialization;

namespace Zion
{
    public class SortedList<T> : IList<T>, IBinarySerializable<SortedList<T>, T> where T : IComparable<T>
    {
        #region Data
        private T[] Data;

        private readonly bool IsReference = RuntimeHelpers.IsReferenceOrContainsReferences<T>();

        public int Count { get; private set; }

        public int Capacity => Data.Length;
        public bool IsReadOnly => false;

        #endregion

        #region Constructors
        public SortedList() : this(16) { }

        public SortedList(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Capacity);

            Data = new T[Capacity];
            Count = 0;
        }

        private SortedList(T[] Data, int Count)
        {
            this.Data = Data;
            this.Count = Count;
            Resort();
        }

        #endregion

        #region Indexers
        public T this[int Index]
        {
            get
            {
                return Data[Index];
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);

                bool ValidLeft = Index == 0 || value.CompareTo(Data[Index - 1]) >= 0;
                bool ValidRight = Index == Count - 1 || value.CompareTo(Data[Index + 1]) <= 0;

                if (ValidLeft && ValidRight)
                {
                    Data[Index] = value;
                }
                else
                {
                    RemoveAt(Index);
                    Add(value);
                }
            }
        }

        public T this[Index Index]
        {
            get => this[Index.GetOffset(Count)];
            set => this[Index.GetOffset(Count)] = value;
        }

        public T[] this[Range Range]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Range.Start.GetOffset(Count), Count);
                ArgumentOutOfRangeException.ThrowIfWithout(Range.End.GetOffset(Count), Count);
                return Data[Range];
            }
        }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            return StringFormatter.ToString(this);
        }

        public override bool Equals(object? Object)
        {
            return Object is SortedList<T> SortedList
                && Count == SortedList.Count
                && this.SequenceEqual(SortedList);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IList
        public void Add(T Item)
        {
            EnsureCapacity(Count + 1);
            Insert(Item, GetInsertIndex(Item));
        }

        public void AddRange(ICollection<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            EnsureCapacity(Count + Items.Count);

            foreach (T Item in Items)
            {
                ArgumentNullException.ThrowIfNull(Item);
                Insert(Item, GetInsertIndex(Item));
            }
        }

        public void Insert(int TargetIndex, T Item)
        {
            ArgumentNullException.ThrowIfNull(Item);

            if (TargetIndex < 0 || TargetIndex > Count)
            {
                Add(Item);
                return;
            }

            bool ValidLeft  = TargetIndex == 0     || Item.CompareTo(Data[TargetIndex - 1]) >= 0;
            bool ValidRight = TargetIndex == Count || Item.CompareTo(Data[TargetIndex])     <= 0;

            if (ValidLeft && ValidRight)
            {
                Insert(Item, TargetIndex);
            }
            else
            {
                Add(Item);
            }
        }


        public int IndexOf(T Item)
        {
            if (Count == 0)
            {
                return -1;
            }

            int Min = 0;
            int Max = Count - 1;

            while (Min <= Max)
            {
                int Target = Min + ((Max - Min) >> 1);
                int Comparing = Item.CompareTo(Data[Target]);

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

        public bool Contains(T Item)
        {
            return IndexOf(Item) != -1;
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfNegative(ArrayIndex);
            ArgumentException.ThrowIf(Array.Length - ArrayIndex < Count, "Target array is too small");
            System.Array.Copy(Data, 0, Array, ArrayIndex, Count);
        }


        public bool Remove(T Item)
        {
            int Index = IndexOf(Item);
            if (Index == -1)
            {
                return false;
            }
            RemoveAt(Index);
            return true;
        }

        public void RemoveAt(int Index)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);

            for (int i = Index; i < Count - 1; i++)
            {
                Data[i] = Data[i + 1];
            }

            Count--;
            if (IsReference)
            {
                Data[Count] = default!;
            }
        }

        public void RemoveRange(int Start, int Count)
        {
            ArgumentException.ThrowIf(Start < 0 || Start > this.Count, $"Start(={Start}) out of range [0..{this.Count})");
            ArgumentException.ThrowIf(Count < 0 || Start + Count > this.Count, $"Count(={Count}) out of range [0..{this.Count})");

            if (Count == 0)
            {
                return;
            }

            int End = Start + Count;

            foreach (int i in ZEnumerable.Range(this.Count - End))
            {
                Data[Start + i] = Data[End + i];
            }

            this.Count -= Count;

            if (IsReference)
            {
                Array.Clear(Data, this.Count, Count);
            }
        }

        public void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            if (IsReference)
            {
                Array.Clear(Data, 0, Count);
            }

            Count = 0;
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return Data[i];
            }
        }

        #endregion

        #region BinarySerializable
        public void Write(BinaryWriter Writer, Action<T> Write)
        {
            //TODO: SortedList<T>.Write
            Writer.Write(Count);

            for (int i = 0; i < Count; i++)
            {
                Write(Data[i]);
            }
        }

        public static SortedList<T> Read(BinaryReader Reader, Func<T> Read)
        {
            //TODO: SortedList<T>.Read
            int Count = Reader.ReadInt32();
            T[] Data = new T[Count];

            for (int i = 0; i < Count; i++)
            {
                Data[i] = Read();
            }

            return new SortedList<T>(Data, Count);
        }

        #endregion

        #region PublicMethods
        public T[] ToArray()
        {
            T[] Array = new T[Count];
            System.Array.Copy(Data, Array, Count);
            return Array;
        }

        public List<T> ToList()
        {
            List<T> List = new List<T>(Count);
            foreach (T Item in this)
            {
                List.Add(Item);
            }
            return List;
        }


        public int FirstAfter(T Item)
        {
            if (Count == 0) { return -1; }

            int Index = GetInsertIndex(Item);
            
            if (Index < Count && Item.CompareTo(Data[Index]) == 0)
            {
                Index++;
            }

            return Index < Count ? Index : -1;
        }


        public void EnsureCapacity(int Capacity)
        {
            if (Capacity <= Data.Length)
            {
                return;
            }

            int NewCapacity = Math.Max(Data.Length * 2, Capacity);
            Array.Resize(ref Data, NewCapacity);
        }


        public void Resort()
        {
            Array.Sort(Data, 0, Count);
        }

        #endregion

        #region PrivateMethods
        private int GetInsertIndex(T Item)
        {
            if (Count == 0)
            {
                return 0;
            }

            int Min = 0;
            int Max = Count - 1;

            while (Min <= Max)
            {
                int Target = Min + ((Max - Min) >> 1);
                int Comparing = Item.CompareTo(Data[Target]);

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
            if (Count == Data.Length)
            {
                EnsureCapacity(Count + 1);
            }

            for (int i = Count; i > Index; i--)
            {
                Data[i] = Data[i - 1];
            }

            Data[Index] = Item;
            Count++;
        }

        #endregion
    }
}