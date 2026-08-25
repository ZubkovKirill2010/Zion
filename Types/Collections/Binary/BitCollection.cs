using System.Collections;
using static System.Net.WebRequestMethods;

namespace Zion
{
    public abstract class BitCollection : IEnumerable<bool>
    {
        #region Data & Properties
        protected byte[] Data;

        public abstract int Count { get; }

        #endregion

        #region Indexers
        public bool this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
                return Data[Index >> 3].GetBit(Index & 0b111);
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
                int ByteIndex = Index >> 3;
                Data[ByteIndex] = Data[ByteIndex].SetBit(Index & 0b111, value);
            }
        }

        public bool this[Index Index]
        {
            get => this[Index.GetOffset(Count)];
            set => this[Index.GetOffset(Count)] = value;
        }

        #endregion

        #region Operators

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            int TotalCount = Count + GetByteCount() + 1;

            return string.Create
            (
                TotalCount,
                (Data, Count),
                static (Span, Data) =>
                {
                    var Source = Data.Data;
                    var Count  = Data.Count;

                    int ByteCount = Count >> 3;
                    int Remainders = Count & 0b111;

                    int Index = 1;

                    for (int i = 0; i < ByteCount; i++)
                    {
                        foreach (bool Bit in Source[i].EnumerateBits())
                        {
                            Span[Index++] = Bit ? '1' : '0';
                        }
                        Span[Index++] = '_';
                    }

                    if (Remainders > 0)
                    {
                        foreach (bool Bit in Source[^1].EnumerateBits(Remainders))
                        {
                            Span[Index++] = Bit ? '1' : '0';
                        }
                    }

                    Span[0] = '[';
                    Span[^1] = ']';
                }
            );
        }

        public override bool Equals(object? Object)
        {
            return Object is BitCollection BitCollection && this == BitCollection;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data, Count);
        }

        #endregion

        #region PublicMethods
        public int GetByteCount()
        {
            return GetByteCount(Count);
        }


        public bool Contains(int Start, int Count, bool Target)
        {

        }

        public void Fill(int Start, int Count, bool Value)
        {

        }

        public bool TryFindShortestSequence(int BitCount, bool TargetBit, out int SequenceStart)
        {
            return TryFindShortestSequence(BitCount, TargetBit, 0, Count, out SequenceStart);
        }

        public bool TryFindShortestSequence(int BitCount, bool TargetBit, int Count, out int SequenceStart)
        {

        }


        public Span<byte> AsSpan()
        {
            return Data.AsSpan(0, GetByteCount());
        }

        public byte[] ToByteArray()
        {
            int Count = this.Count;
            int ByteCount = GetByteCount(Count);

            byte[] Result = new byte[ByteCount];

            Array.Copy(Data, Result, ByteCount);

            return Result;
        }

        public bool[] ToBooleanArray()
        {
            bool[] Result = new bool[Count];

            int Index = 0;

            foreach (bool Bit in this)
            {
                if (Bit)
                {
                    Result[Index] = true;
                }

                Index++;
            }

            return Result;
        }

        #endregion

        #region ProtectedMethods
        protected void ForEach(Action<byte> FullBytes, Action<byte, int> LastByte)
        {
            ArgumentNullException.ThrowIfNull(FullBytes);
            ArgumentNullException.ThrowIfNull(LastByte);

            var Data = this.Data;

            int BitCount = Count;
            int ByteCount = Count >> 3;
            int Remainders = Count & 0b111;

            for (int i = 0; i < ByteCount; i++)
            {
                FullBytes.Invoke(Data[i]);
            }

            if (Remainders > 0)
            {
                LastByte.Invoke(Data[^1], Remainders);
            }
        }

        protected void ForEach(int Start, int Count, Action<byte, int> FirstByte, Action<byte> FullBytes, Action<byte, int> LastByte)
        {

        }

        protected void Convert(Func<byte, byte> FullBytes, Func<byte, int, byte> LastByte)
        {

        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<bool> GetEnumerator()
        {
            var Source = Data;
            var Count = this.Count;

            int ByteCount = Count >> 3;
            int Remainders = Count & 0b111;

            for (int i = 0; i < ByteCount; i++)
            {
                foreach (bool Bit in Source[i].EnumerateBits())
                {
                    yield return Bit;
                }
            }

            if (Remainders > 0)
            {
                foreach (bool Bit in Source[^1].EnumerateBits(Remainders))
                {
                    yield return Bit;
                }
            }
        }

        #endregion

        #region PrivateMethods
        private static int GetByteCount(int BitCount)
        {
            return (BitCount + 7) >> 3;
        }

        #endregion
    }
}