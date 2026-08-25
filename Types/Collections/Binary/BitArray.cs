using Zion.Serialization;

namespace Zion
{
    public sealed class BitArray : BitCollection, IBinarySerializable<BitArray>
    {
        #region Data
        public static readonly BitArray Empty = new BitArray(0);

        public override int Count { get; }

        #endregion

        #region Constructors
        public BitArray(int Length)
        {
            this.Data = new byte[GetByteCount(Length)];
            this.Count = Length;
        }

        public BitArray(byte[] Data, int Length)
        {
            ArgumentNullException.ThrowIfNull(Data);
            ArgumentOutOfRangeException.ThrowIfWithout(Length >> 3, Data.Length);

            this.Data   = ZArray.Clone(Data);
            this.Count = Length;
        }

        private BitArray(int Length, byte[] Data)
        {
            this.Count = Length;
            this.Data = Data;
        }

        #endregion

        #region Operators
        public static bool operator ==(BitArray A, BitArray B)
        {
            if (object.CompareReferences(A, B, out bool ReferenceComprasion))
            {
                return ReferenceComprasion;
            }
            if (A.Count != B.Count)
            {
                return false;
            }

            int Count = A.Count & 0b111;
            byte[] ABuffer = A.Data;
            byte[] BBuffer = B.Data;

            for (int i = 0; i < Count; i++)
            {
                if (ABuffer[i] != BBuffer[i])
                {
                    return false;
                }
            }

            int LastByteOffset = 8 - A.GetLastByteLength();
            return ABuffer[^1] >> LastByteOffset == ABuffer[^1] >> LastByteOffset;
        }

        public static bool operator !=(BitArray A, BitArray B)
        {
            return !(A == B);
        }


        public static BitArray operator &(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A & B));
        }

        public static BitArray operator |(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A | B));
        }

        public static BitArray operator ^(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A ^ B));

        }

        public static BitArray operator ~(BitArray Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            byte[] Source = Value.Data;
            byte[] Result = new byte[Source.Length];
            int Length = Value.Count;

            for (int i = 0; i < Length; i++)
            {
                Result[i] = (byte)~Source[i];
            }

            return new BitArray(Length, Result);
        }

        #endregion

        #region StaticMethods
        public static BitArray Resize(BitArray Source, int NewLength)
        {
            ArgumentNullException.ThrowIfNull(Source);

            if (NewLength <= 0)
            {
                Source = Empty;
            }

            byte[] Result = new byte[GetByteCount(NewLength)];

            Array.Copy(Source.Data, Result, Math.Min(Source.Count, NewLength));

            return new BitArray(NewLength, Result);
        }

        #endregion

        #region PublicMethods
        public BitArray Clone()
        {
            return new BitArray(Data, Count);
        }

        public BitList ToBitList()
        {
            return new BitList(Data, Count);
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
            return TryFindShortestSequence(this.Count, TargetBit, 0, Count, out SequenceStart);
        }

        public bool TryFindShortestSequence(int Length, bool TargetBit, int Start, int Count, out int SequenceStart)
        {
            //TODO
            //Найти кратчайшую последовательсоть битов и вернуть позицию начала этой последовательности
            SequenceStart = -1;
            return false;
        }


        public Span<byte> AsSpan()
        {
            return Data.AsSpan();
        }

        public byte[] ToByteArray()
        {
            return ZArray.Clone(Data);
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

        #region IBinarySerializable
        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Count);
            Writer.Write(Data, 0, GetByteCount(Count));
        }

        public static BitArray Read(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();
            byte[] Data = Reader.ReadBytes(GetByteCount(Count));

            return new BitArray(Count, Data);
        }

        #endregion

        #region PrivateMethods
        private int GetLastByteLength()
        {
            return Data.Length - Count;
        }

        private static int GetByteCount(int Count)
        {
            return (Count + 7) >> 3;
        }

        private static BitArray ConvertBits(BitArray A, BitArray B, Func<byte, byte, byte> Convert)
        {
            ArgumentNullException.ThrowIfNull(A);
            ArgumentNullException.ThrowIfNull(B);

            int MinLength = Math.Min(A.Data.Length, B.Data.Length);
            int MaxLength = Math.Max(A.Data.Length, B.Data.Length);
            byte[] Result = new byte[MaxLength];
            Span<byte> ASpan = A.AsSpan();
            Span<byte> BSpan = B.AsSpan();

            for (int i = 0; i < MinLength; i++)
            {
                Result[i] = Convert(ASpan[i], BSpan[i]);
            }

            return new BitArray(MaxLength, Result);
        }

        #endregion
    }
}