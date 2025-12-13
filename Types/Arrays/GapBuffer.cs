using System.Collections;

namespace Zion
{
    public sealed class GapBuffer<T> : IList<T>, IBinaryGeneric<GapBuffer<T>, T>
    {
        private T[] Buffer;
        private int GapStart;
        private int GapEnd;
        private int Length;

        private readonly bool TIsReference;

        private int GapLength => GapEnd - GapStart;

        public int Count => Length;
        public int Capacity => Buffer.Length;
        public bool IsReadOnly => false;


        public GapBuffer() : this(50) { }
        public GapBuffer(int Capacity)
        {
            Buffer = new T[Capacity];
            GapStart = 0;
            GapEnd = Capacity;
            Length = 0;

            TIsReference = typeof(T).IsClass;
        }

        public T this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);
                return Buffer[GetIndex(Index)];
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);
                Buffer[GetIndex(Index)] = value;
            }
        }


        public void Write(BinaryWriter Writer, Action<BinaryWriter, T> WriteObject)
        {
            Writer.Write(Length);
            foreach (T? item in this)
            {
                WriteObject(Writer, item);
            }
        }

        public static GapBuffer<T> Read(BinaryReader Reader, Func<BinaryReader, T> ReadObject)
        {
            int Length = Reader.ReadInt32();

            GapBuffer<T> Buffer = new GapBuffer<T>(Length);

            for (int i = 0; i < Length; i++)
            {
                Buffer.Add(ReadObject(Reader));
            }

            return Buffer;
        }


        public void Add(T Item)
        {
            Insert(Length, Item);
        }

        public void Add(IEnumerable<T> Values, int Count)
        {
            EnsureCapacity(Length + Count);
            MoveGapTo(Length);

            foreach (T Item in Values.Limit(Count))
            {
                Buffer[GapStart] = Item;
                GapStart++;
                Length++;
            }
        }

        public void Insert(int Index, T Item)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);

            EnsureCapacity(Length + 1);
            MoveGapTo(Index);

            Buffer[GapStart] = Item;
            GapStart++;
            Length++;
        }

        public void Insert(int Index, IEnumerable<T> Values, int Count)
        {
            EnsureCapacity(Length + Count);
            MoveGapTo(Index);

            foreach (T Item in Values.Limit(Count))
            {
                Buffer[GapStart] = Item;
                GapStart++;
                Length++;
            }
        }

        public int IndexOf(T Target)
        {
            int Index = 0;
            foreach (T item in this)
            {
                if (EqualityComparer<T>.Default.Equals(item, Target))
                {
                    return Index;
                }
                Index++;
            }
            return -1;
        }

        public bool Contains(T Target)
        {
            return IndexOf(Target) >= 0;
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfNegative(ArrayIndex);
            if (Array.Length - ArrayIndex < Length)
            {
                throw new ArgumentException("Target array too small");
            }

            int Index = ArrayIndex;
            foreach (T? item in this)
            {
                Array[Index++] = item;
            }
        }

        public GapBuffer<T> Clone()
        {
            GapBuffer<T> Clone = new GapBuffer<T>(Buffer.Length)
            {
                GapStart = GapStart,
                GapEnd = GapEnd,
                Length = Length,
            };
            Array.Copy(Buffer, Clone.Buffer, Buffer.Length);
            return Clone;
        }

        public void RemoveAt(int Index)
        {
            if (Index < 0 || Index >= Length)
            {
                throw new ArgumentOutOfRangeException($"Index(={Index}) out of range 0, Count(={Count}");
            }

            MoveGapTo(Index + 1);
            GapStart--;
            Length--;

            if (TIsReference)
            {
                Buffer[GapStart] = default;
            }
        }

        public bool Remove(T Item)
        {
            int Index = IndexOf(Item);
            if (Index != -1)
            {
                RemoveAt(Index);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            GapStart = 0;
            GapEnd = Buffer.Length;
            Length = 0;

            if (TIsReference)
            {
                Array.Clear(Buffer, 0, Buffer.Length);
            }
        }


        public void Resize(int NewCapacity)
        {
            if (Buffer.Length >= NewCapacity) { return; }
            EnsureCapacity(NewCapacity);
        }


        public IEnumerator<T> GetEnumerator()
        {
            int Index = 0;

            while (Index < GapStart)
            {
                yield return Buffer[Index++];
            }

            Index = GapEnd;

            while (Index < Buffer.Length)
            {
                yield return Buffer[Index++];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private int GetIndex(int Index)
        {
            return Index < GapStart ? Index : Index + GapLength;
        }

        private void MoveGapTo(int Index)
        {
            if (Index == GapStart) { return; }

            if (Index < GapStart)
            {
                int Shift = GapStart - Index;
                Array.Copy(Buffer, Index, Buffer, GapEnd - Shift, Shift);
                GapStart = Index;
                GapEnd -= Shift;
            }
            else
            {
                int Shift = Index - GapStart;
                Array.Copy(Buffer, GapEnd, Buffer, GapStart, Shift);
                GapStart = Index;
                GapEnd += Shift;
            }
        }

        private void EnsureCapacity(int TargetCapacity)
        {
            if (Buffer.Length >= TargetCapacity + GapLength)
            {
                return;
            }

            int NewCapacity = Math.Max(Buffer.Length * 2, TargetCapacity + GapLength);
            int RightPartLength = Buffer.Length - GapEnd;
            T[] NewBuffer = new T[NewCapacity];

            Array.Copy(Buffer, 0, NewBuffer, 0, GapStart);
            Array.Copy(Buffer, GapEnd, NewBuffer, NewCapacity - RightPartLength, RightPartLength);

            Buffer = NewBuffer;
            GapEnd = NewCapacity - RightPartLength;
        }
    }
}