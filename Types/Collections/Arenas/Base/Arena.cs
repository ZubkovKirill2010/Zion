using System.Drawing;
using System.Runtime.CompilerServices;

namespace Zion
{
    //TODO: Полностью убрать доступ к арене у Disposed Collection (чтобы копия структуры также не имела доступ).

    public sealed class Arena<T>
    {
        #region Constants
        internal const int BinaryGroupSize = 4;
        internal const int GroupSize = 1 << BinaryGroupSize;

        internal const int BinaryBufferAccuracry = 9;
        internal const int BufferAccuracry = 1 << BinaryBufferAccuracry;

        #endregion

        #region Data
        private T[] Data;
        private BitArray BitMap;
        private int Count;

        #endregion

        #region Properties
        public int Capacity
        {
            get => Data.Length;
            set
            {
                if (value > Data.Length)
                {
                    value = RoundToBufferSize(value);
                    Array.Resize(ref Data, value);
                    BitMap = BitArray.Resize(BitMap, value >> BinaryGroupSize);
                }
            }
        }

        #endregion

        #region Constructors
        public Arena() : this(1024) { }

        public Arena(int Capacity)
        {
            Capacity = RoundToBufferSize(Math.Max(1024, Capacity));
            this.Data     = new T[Capacity];
            this.BitMap   = new (RoundToGroup(Capacity));
        }

        #endregion

        #region Indexers
        internal T this[int Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }

        internal T this[Index Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }

        #endregion

        #region PublicMethods
        public ArenaArray<T> GetArray(int Size)
        {
            return Allocate<ArenaArray<T>>(Size, static Span => new(Span));
        }

        public ArenaBuffer<T> GetBuffer(int Size)
        {
            return Allocate<ArenaBuffer<T>>(RoundToGroup(Size), static Span => new(Span));
        }

        public ArenaList<T> GetList(int Size)
        {
            return Allocate<ArenaList<T>>(RoundToGroup(Size), static Span => new(Span));
        }

        public ArenaQueue<T> GetQueue(int Size)
        {
            return Allocate<ArenaQueue<T>>(RoundToGroup(Size), static Span => new(Span));
        }

        public A Allocate<A>(int Size, Func<ArenaSpan<T>, A> Fabric) where A : ArenaCollection<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);

            int Start = GetFreeArea(Size);

            MarkArea(Start, Size, true);
            UpdateCount(Start + Size);

            var Span = new ArenaSpan<T>(this, Start, Size);

            return Fabric(Span);
        }


        public T[] ToArray()
        {
            return ZArray.Clone(Data);
        }

        public T[] ToArray(int Start, int Length)
        {
            return ZArray.GetSubArray(Data, Start, Length);
        }

        #endregion

        #region InternalMethods
        internal Span<T> AsSpan(ArenaCollection<T> Collection)
        {
            CheckCollection(Collection);
            return Data.AsSpan(Collection.Start, Collection.Length);
        }

        internal Span<T> AsSpan(ArenaCollection<T> Collection, int Start, int Count)
        {
            CheckCollection(Collection);

            int ArenaLength = Collection.Length;

            ArgumentOutOfRangeException.ThrowIfWithout(Start, ArenaLength);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, ArenaLength);

            return Data.AsSpan(Collection.Start + Start, Count);
        }

        internal Memory<T> AsMemory(ArenaCollection<T> Collection)
        {
            CheckCollection(Collection);
            return Data.AsMemory(Collection.Start, Collection.Length);
        }

        internal void Release(ArenaCollection<T> Collection)
        {
            CheckCollection(Collection);
            MarkArea(Collection.Start, Collection.Length, false);

            if (IsLastCollection(Collection))
            {
                Count = Collection.Start;
            }

            Collection.ResetToZero();
        }

        internal Segment Expand(ArenaCollection<T> Collection, int Additional)
        {
            if (TryExpand(Collection, Additional, out var Expanded))
            {
                return Expanded;
            }

            int Start = GetFreeArea(Additional);

            MarkArea(Start, Additional, true);
            UpdateCount(Start + Additional);

            CopyTo(Collection, Start);
            MarkArea(Collection, false);

            return new Segment(Start, Additional);
        }

        #endregion

        #region PrivateMethods
        private void CheckCollection(ArenaCollection<T> Collection)
        {
            if (!ReferenceEquals(this, Collection.Source))
            {
                throw new InvalidOperationException("Arena not contains this ArenaSpan");
            }
            if (Collection.IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Collection));
            }
        }

        private void UpdateCount(int NewCount)
        {
            if (NewCount > Count)
            {
                Count = RoundToGroup(NewCount);
            }
        }

        private void MarkArea(ArenaCollection<T> Area, bool Busy)
        {
            MarkArea(Area.Start, Area.Length, Busy);
        }

        private void MarkArea(int Start, int Count, bool Busy)
        {
            BitMap.Fill(FloorToGroup(Start), RoundToGroup(Count), Busy);
        }

        private void CopyTo(ArenaCollection<T> Source, int Destination)
        {
            var SourceSpan = Data.AsSpan(Source.Start, Source.Length);
            var DestinationSpan = Data.AsSpan(Destination, Source.Length);

            SourceSpan.CopyTo(DestinationSpan);
        }

        private bool IsLastCollection(ArenaCollection<T> Collection)
        {
            return Collection.Start + Collection.Length == Count;
        }

        private bool TryExpand(ArenaCollection<T> Collection, int Additional, out Segment Expanded)
        {
            int SpanEnd = Collection.Start + Collection.Length;
            int Start = RoundToGroup(SpanEnd);
            int End = RoundToGroup(SpanEnd + Additional);
              
            if (Start == End || !BitMap.Contains(Start, End - Start, true))
            {
                Expanded = new(Collection.Start, SpanEnd + Additional - Collection.Start);
                return true;
            }

            Expanded = default;
            return false;
        }

        private int GetFreeArea(int Size)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);
            if (Size == 0)
            {
                return Count;
            }

            if (BitMap.TryFindShortestSequence(RoundToGroup(Size), false, Count, out Segment Sequence))
            {
                return Sequence.Start;
            }

            Capacity += Size;
            return Count;
        }

        #endregion

        #region PublicStaticMethods
        public static int RoundToGroup(int Count)
        {
            return (Count + GroupSize - 1) >> BinaryGroupSize;
        }

        public static int FloorToGroup(int Count)
        {
            return Count >> BinaryGroupSize;
        }

        public static int RoundToBufferSize(int Count)
        {
            return (Count + BufferAccuracry - 1) & ~(BufferAccuracry - 1);
        }

        #endregion
    }
}